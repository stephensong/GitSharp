﻿/*
 * Copyright (C) 2008, Robin Rosenberg <robin.rosenberg@dewire.com>
 * Copyright (C) 2008, Shawn O. Pearce <spearce@spearce.org>
 *
 * All rights reserved.
 *
 * Redistribution and use in source and binary forms, with or
 * without modification, are permitted provided that the following
 * conditions are met:
 *
 * - Redistributions of source code must retain the above copyright
 *   notice, this list of conditions and the following disclaimer.
 *
 * - Redistributions in binary form must reproduce the above
 *   copyright notice, this list of conditions and the following
 *   disclaimer in the documentation and/or other materials provided
 *   with the distribution.
 *
 * - Neither the name of the Git Development Community nor the
 *   names of its contributors may be used to endorse or promote
 *   products derived from this software without specific prior
 *   written permission.
 *
 * THIS SOFTWARE IS PROVIDED BY THE COPYRIGHT HOLDERS AND
 * CONTRIBUTORS "AS IS" AND ANY EXPRESS OR IMPLIED WARRANTIES,
 * INCLUDING, BUT NOT LIMITED TO, THE IMPLIED WARRANTIES
 * OF MERCHANTABILITY AND FITNESS FOR A PARTICULAR PURPOSE
 * ARE DISCLAIMED. IN NO EVENT SHALL THE COPYRIGHT OWNER OR
 * CONTRIBUTORS BE LIABLE FOR ANY DIRECT, INDIRECT, INCIDENTAL,
 * SPECIAL, EXEMPLARY, OR CONSEQUENTIAL DAMAGES (INCLUDING, BUT
 * NOT LIMITED TO, PROCUREMENT OF SUBSTITUTE GOODS OR SERVICES;
 * LOSS OF USE, DATA, OR PROFITS; OR BUSINESS INTERRUPTION) HOWEVER
 * CAUSED AND ON ANY THEORY OF LIABILITY, WHETHER IN CONTRACT,
 * STRICT LIABILITY, OR TORT (INCLUDING NEGLIGENCE OR OTHERWISE)
 * ARISING IN ANY WAY OUT OF THE USE OF THIS SOFTWARE, EVEN IF
 * ADVISED OF THE POSSIBILITY OF SUCH DAMAGE.
 */

using System.IO;
using GitSharp.Transport;
using NUnit.Framework;

namespace GitSharp.Tests.Transport
{

    [TestFixture]
    public class IndexPackTest : RepositoryTestCase
    {
        private FileInfo getPack(string name)
        {
            return new FileInfo(db.ObjectsDirectory + "/pack/" + name);
        }

        [Test]
        public void test000_FirstPack()
        {
            FileInfo packFile = getPack("pack-34be9032ac282b11fa9babdc2b2a93ca996c9c2f.pack");
            Stream @is = packFile.Open(System.IO.FileMode.Open, FileAccess.Read);
            try
            {
                FileInfo tmppack = new FileInfo(Path.Combine(trash.ToString(), "tmp_pack1"));
                FileInfo idxFile = new FileInfo(Path.Combine(trash.ToString(), "tmp_pack1.idx"));
                FileInfo tmpPackFile = new FileInfo(Path.Combine(trash.ToString(), "tmp_pack1.pack"));

                tmppack.Create().Close();
                idxFile.Create().Close();
                tmpPackFile.Create().Close();

                IndexPack pack = new IndexPack(db, @is, tmppack);
                pack.index(new TextProgressMonitor());
                PackFile file = new PackFile(idxFile, tmpPackFile);

                Assert.True(file.HasObject(ObjectId.FromString("4b825dc642cb6eb9a060e54bf8d69288fbee4904")));
                Assert.True(file.HasObject(ObjectId.FromString("540a36d136cf413e4b064c2b0e0a4db60f77feab")));
                Assert.True(file.HasObject(ObjectId.FromString("5b6e7c66c276e7610d4a73c70ec1a1f7c1003259")));
                Assert.True(file.HasObject(ObjectId.FromString("6ff87c4664981e4397625791c8ea3bbb5f2279a3")));
                Assert.True(file.HasObject(ObjectId.FromString("82c6b885ff600be425b4ea96dee75dca255b69e7")));
                Assert.True(file.HasObject(ObjectId.FromString("902d5476fa249b7abc9d84c611577a81381f0327")));
                Assert.True(file.HasObject(ObjectId.FromString("aabf2ffaec9b497f0950352b3e582d73035c2035")));
                Assert.True(file.HasObject(ObjectId.FromString("c59759f143fb1fe21c197981df75a7ee00290799")));
            }
            finally
            {
                @is.Close();
            }
        }

        [Test]
        public void test001_SecondPack()
        {
            FileInfo packFile = getPack("pack-df2982f284bbabb6bdb59ee3fcc6eb0983e20371.pack");
            Stream @is = packFile.Open(System.IO.FileMode.Open, FileAccess.Read);
            try
            {
                FileInfo tmppack = new FileInfo(Path.Combine(trash.ToString(), "tmp_pack2"));
                FileInfo idxFile = new FileInfo(Path.Combine(trash.ToString(), "tmp_pack2.idx"));
                FileInfo tmpPackFile = new FileInfo(Path.Combine(trash.ToString(), "tmp_pack2.pack"));

                tmppack.Create().Close();
                idxFile.Create().Close();
                tmpPackFile.Create().Close();

                IndexPack pack = new IndexPack(db, @is, tmppack);
                pack.index(new TextProgressMonitor());
                PackFile file = new PackFile(idxFile, tmpPackFile); 
                
                Assert.True(file.HasObject(ObjectId.FromString("02ba32d3649e510002c21651936b7077aa75ffa9")));
                Assert.True(file.HasObject(ObjectId.FromString("0966a434eb1a025db6b71485ab63a3bfbea520b6")));
                Assert.True(file.HasObject(ObjectId.FromString("09efc7e59a839528ac7bda9fa020dc9101278680")));
                Assert.True(file.HasObject(ObjectId.FromString("0a3d7772488b6b106fb62813c4d6d627918d9181")));
                Assert.True(file.HasObject(ObjectId.FromString("1004d0d7ac26fbf63050a234c9b88a46075719d3")));
                Assert.True(file.HasObject(ObjectId.FromString("10da5895682013006950e7da534b705252b03be6")));
                Assert.True(file.HasObject(ObjectId.FromString("1203b03dc816ccbb67773f28b3c19318654b0bc8")));
                Assert.True(file.HasObject(ObjectId.FromString("15fae9e651043de0fd1deef588aa3fbf5a7a41c6")));
                Assert.True(file.HasObject(ObjectId.FromString("16f9ec009e5568c435f473ba3a1df732d49ce8c3")));
                Assert.True(file.HasObject(ObjectId.FromString("1fd7d579fb6ae3fe942dc09c2c783443d04cf21e")));
                Assert.True(file.HasObject(ObjectId.FromString("20a8ade77639491ea0bd667bf95de8abf3a434c8")));
                Assert.True(file.HasObject(ObjectId.FromString("2675188fd86978d5bc4d7211698b2118ae3bf658")));
                // and lots more...
            }
            finally
            {
                @is.Close();
            }
        }
    }

}