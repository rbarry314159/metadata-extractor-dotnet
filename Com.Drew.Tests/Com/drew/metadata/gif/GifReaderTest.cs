/*
 * Copyright 2002-2015 Drew Noakes
 *
 *    Modified by Yakov Danilov <yakodani@gmail.com> for Imazen LLC (Ported from Java to C#)
 *    Licensed under the Apache License, Version 2.0 (the "License");
 *    you may not use this file except in compliance with the License.
 *    You may obtain a copy of the License at
 *
 *        http://www.apache.org/licenses/LICENSE-2.0
 *
 *    Unless required by applicable law or agreed to in writing, software
 *    distributed under the License is distributed on an "AS IS" BASIS,
 *    WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
 *    See the License for the specific language governing permissions and
 *    limitations under the License.
 *
 * More information about this project is available at:
 *
 *    https://drewnoakes.com/code/exif/
 *    https://github.com/drewnoakes/metadata-extractor
 */

using Com.Drew.Lang;
using JetBrains.Annotations;
using NUnit.Framework;
using Sharpen;

namespace Com.Drew.Metadata.Gif
{
    /// <author>Drew Noakes https://drewnoakes.com</author>
    public class GifReaderTest
    {
        /// <exception cref="System.Exception"/>
        [NotNull]
        public static GifHeaderDirectory ProcessBytes([NotNull] string file)
        {
            Metadata metadata = new Metadata();
            InputStream stream = new FileInputStream(file);
            new GifReader().Extract(new StreamReader(stream), metadata);
            stream.Close();
            GifHeaderDirectory directory = metadata.GetFirstDirectoryOfType<GifHeaderDirectory>();
            Assert.IsNotNull(directory);
            return directory;
        }

        /// <exception cref="System.Exception"/>
        [Test]
        public virtual void TestMsPaintGif()
        {
            GifHeaderDirectory directory = ProcessBytes("Tests/Data/mspaint-10x10.gif");
            Assert.IsFalse(directory.HasErrors());
            Assert.AreEqual("89a", directory.GetString(GifHeaderDirectory.TagGifFormatVersion));
            Assert.AreEqual(10, directory.GetInt(GifHeaderDirectory.TagImageWidth));
            Assert.AreEqual(10, directory.GetInt(GifHeaderDirectory.TagImageHeight));
            Assert.AreEqual(256, directory.GetInt(GifHeaderDirectory.TagColorTableSize));
            Assert.IsFalse(directory.GetBoolean(GifHeaderDirectory.TagIsColorTableSorted));
            Assert.AreEqual(8, directory.GetInt(GifHeaderDirectory.TagBitsPerPixel));
            Assert.IsTrue(directory.GetBoolean(GifHeaderDirectory.TagHasGlobalColorTable));
            Assert.AreEqual(0, directory.GetInt(GifHeaderDirectory.TagTransparentColorIndex));
        }

        /// <exception cref="System.Exception"/>
        [Test]
        public virtual void TestPhotoshopGif()
        {
            GifHeaderDirectory directory = ProcessBytes("Tests/Data/photoshop-8x12-32colors-alpha.gif");
            Assert.IsFalse(directory.HasErrors());
            Assert.AreEqual("89a", directory.GetString(GifHeaderDirectory.TagGifFormatVersion));
            Assert.AreEqual(8, directory.GetInt(GifHeaderDirectory.TagImageWidth));
            Assert.AreEqual(12, directory.GetInt(GifHeaderDirectory.TagImageHeight));
            Assert.AreEqual(32, directory.GetInt(GifHeaderDirectory.TagColorTableSize));
            Assert.IsFalse(directory.GetBoolean(GifHeaderDirectory.TagIsColorTableSorted));
            Assert.AreEqual(5, directory.GetInt(GifHeaderDirectory.TagBitsPerPixel));
            Assert.IsTrue(directory.GetBoolean(GifHeaderDirectory.TagHasGlobalColorTable));
            Assert.AreEqual(8, directory.GetInt(GifHeaderDirectory.TagTransparentColorIndex));
        }
    }
}