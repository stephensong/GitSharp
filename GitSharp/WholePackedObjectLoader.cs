﻿/*
 * Copyright (C) 2008, Shawn O. Pearce <spearce@spearce.org>
 * Copyright (C) 2009, Henon <meinrad.recheis@gmail.com>
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

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GitSharp.Exceptions;
using System.IO;

namespace GitSharp
{
    /** Reader for a non-delta (just deflated) object in a pack file. */
    public class WholePackedObjectLoader : PackedObjectLoader
    {
        private static int OBJ_COMMIT = Constants.OBJ_COMMIT;

        public WholePackedObjectLoader(PackFile pr, long dataOffset, long objectOffset, int type, int size)
            : base(pr, dataOffset, objectOffset)
        {
            objectType = type;
            objectSize = size;
        }


        public override void materialize(WindowCursor curs)
        {
            if (cachedBytes != null)
            {
                return;
            }

            if (objectType != OBJ_COMMIT)
            {
                UnpackedObjectCache.Entry cache = pack.readCache(dataOffset);
                if (cache != null)
                {
                    curs.release();
                    cachedBytes = cache.data;
                    return;
                }
            }

            try
            {
                cachedBytes = pack.decompress(dataOffset, objectSize, curs);
                curs.release();
                if (objectType != OBJ_COMMIT)
                    pack.saveCache(dataOffset, cachedBytes, objectType);
            }
            catch (IOException dfe)
            {
                CorruptObjectException coe;
                coe = new CorruptObjectException("object at " + dataOffset + " in "
                        + pack.File.FullName + " has bad zlib stream", dfe);
                throw coe;
            }
        }


        public override int getRawType()
        {
            return objectType;
        }


        public override long getRawSize()
        {
            return objectSize;
        }


        public override ObjectId getDeltaBase()
        {
            return null;
        }
    }
}
