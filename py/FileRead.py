import os,sys
from PIL import Image
from io import BytesIO

def convertToBmp(im):
    with BytesIO() as f:
        im.save(f, format='BMP')
        return f.getvalue()

for file in os.listdir("..\screenshots\Resize"):
    if file.endswith(".png"):
        filepath = os.path.join("..\screenshots\Resize", file)
        print(filepath)
        imagefile = Image.open(filepath)
        print(imagefile.info, imagefile.size, imagefile.format)
        bmp = convertToBmp(imagefile)
        bmpbytes = bytearray(bmp)
        bytecount = len(bmpbytes)
        print(bytecount)
        #for b in bmpbytes:
        #    print(b)
        imagefile.close()
        #break
