using System;
using System.IO;
using System.Runtime.InteropServices;
using Avalonia.Media.Imaging;
using Gaku.Interfaces;

namespace Gaku.Helpers;

public class MacOSGraphics : IMacOSGraphics
{
    // Import CoreGraphics functions for screen capture
    [DllImport("/System/Library/Frameworks/CoreGraphics.framework/CoreGraphics")]
    private static extern IntPtr CGDisplayCreateImage(uint displayId);

    [DllImport("/System/Library/Frameworks/CoreGraphics.framework/CoreGraphics")]
    private static extern void CFRelease(IntPtr obj);

    [DllImport("/System/Library/Frameworks/ImageIO.framework/ImageIO")]
    private static extern IntPtr CGImageDestinationCreateWithData(IntPtr data, IntPtr type, IntPtr count, IntPtr options);

    [DllImport("/System/Library/Frameworks/CoreFoundation.framework/CoreFoundation")]
    private static extern IntPtr CFDataCreateMutable(IntPtr allocator, IntPtr capacity);

    [DllImport("/System/Library/Frameworks/ImageIO.framework/ImageIO")]
    private static extern void CGImageDestinationAddImage(IntPtr dest, IntPtr image, IntPtr properties);

    [DllImport("/System/Library/Frameworks/ImageIO.framework/ImageIO")]
    private static extern bool CGImageDestinationFinalize(IntPtr dest);

    [DllImport("/System/Library/Frameworks/CoreGraphics.framework/CoreGraphics")]
    private static extern uint CGGetActiveDisplayList(uint maxDisplays, [Out] uint[] activeDisplays, out uint displayCount);

    
    // Heavily Experimental, only captures the primary display for now
    public void TryCaptureSecondaryDisplays()
    {
        uint maxDisplays = 16;
        uint[] activeDisplays = new uint[maxDisplays];
        CGGetActiveDisplayList(maxDisplays, activeDisplays, out uint displayCount);

        for (int i = 0; i < displayCount; i++)
        {
            uint displayId = activeDisplays[i];
            Console.WriteLine($"Trying to capture display: {displayId}");

            IntPtr imageRef = CGDisplayCreateImage(displayId);
            if (imageRef != IntPtr.Zero)
            {
                Console.WriteLine($"Successfully captured display {displayId}");
                CFRelease(imageRef);
                return;
            }
        }

        Console.WriteLine("Failed to capture any display.");
    }
    
    public Bitmap CaptureFullScreen()
    {
        const uint kCGDirectMainDisplay = 2; // Primary display

        IntPtr imageRef = CGDisplayCreateImage(kCGDirectMainDisplay);
        if (imageRef == IntPtr.Zero)
        {
            TryCaptureSecondaryDisplays();
            throw new InvalidOperationException("Failed to capture macOS display image.");
        }

        try
        {
            // Create mutable data object
            IntPtr mutableData = CFDataCreateMutable(IntPtr.Zero, IntPtr.Zero);

            // Create image destination with PNG type
            IntPtr pngType = CFStringCreateWithCString(IntPtr.Zero, "public.png", 0);
            IntPtr imageDestination = CGImageDestinationCreateWithData(mutableData, pngType, (IntPtr)1, IntPtr.Zero);

            if (imageDestination == IntPtr.Zero)
            {
                throw new InvalidOperationException("Failed to create image destination.");
            }

            // Add image to destination and finalize
            CGImageDestinationAddImage(imageDestination, imageRef, IntPtr.Zero);
            if (!CGImageDestinationFinalize(imageDestination))
            {
                throw new InvalidOperationException("Failed to finalize image destination.");
            }

            // Convert to Avalonia Bitmap
            return CFDataToAvaloniaBitmap(mutableData);
        }
        finally
        {
            CFRelease(imageRef); // Clean up the CGImage reference
        }
    }

    [DllImport("/System/Library/Frameworks/CoreFoundation.framework/CoreFoundation")]
    private static extern IntPtr CFStringCreateWithCString(IntPtr allocator, string str, int encoding);

    private Bitmap CFDataToAvaloniaBitmap(IntPtr cfData)
    {
        byte[] buffer = CFDataToByteArray(cfData);
        using var stream = new MemoryStream(buffer);
        return new Bitmap(stream);
    }

    [DllImport("/System/Library/Frameworks/CoreFoundation.framework/CoreFoundation")]
    private static extern IntPtr CFDataGetLength(IntPtr data);

    [DllImport("/System/Library/Frameworks/CoreFoundation.framework/CoreFoundation")]
    private static extern IntPtr CFDataGetBytePtr(IntPtr data);

    private byte[] CFDataToByteArray(IntPtr cfData)
    {
        int length = (int)CFDataGetLength(cfData);
        IntPtr bytePtr = CFDataGetBytePtr(cfData);
        byte[] buffer = new byte[length];
        Marshal.Copy(bytePtr, buffer, 0, length);
        return buffer;
    }
}