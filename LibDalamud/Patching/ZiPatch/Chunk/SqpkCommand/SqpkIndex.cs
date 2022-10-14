﻿using XIVLauncher.Common.Patching.Util;
using XIVLauncher.Common.Patching.ZiPatch.Util;

namespace XIVLauncher.Common.Patching.ZiPatch.Chunk.SqpkCommand
{
    class SqpkIndex : SqpkChunk
    {
        // This is a NOP on recent patcher versions.
        public new static string Command = "I";

        public enum IndexCommandKind : byte
        {
            Add = (byte)'A',
            Delete = (byte)'D'
        }

        public IndexCommandKind IndexCommand { get; protected set; }
        public bool IsSynonym { get; protected set; }
        public SqpackIndexFile TargetFile { get; protected set; }
        public ulong FileHash { get; protected set; }
        public uint BlockOffset { get; protected set; }

        // TODO: Figure out what this is used for
        public uint BlockNumber { get; protected set; }



        public SqpkIndex(ChecksumBinaryReader reader, int offset, int size) : base(reader, offset, size) {}

        protected override void ReadChunk()
        {
            var start = this.Reader.BaseStream.Position;

            IndexCommand = (IndexCommandKind)this.Reader.ReadByte();
            IsSynonym = this.Reader.ReadBoolean();
            this.Reader.ReadByte(); // Alignment

            TargetFile = new SqpackIndexFile(this.Reader);

            FileHash = this.Reader.ReadUInt64BE();

            BlockOffset = this.Reader.ReadUInt32BE();
            BlockNumber = this.Reader.ReadUInt32BE();

            this.Reader.ReadBytes(Size - (int)(this.Reader.BaseStream.Position - start));
        }

        public override string ToString()
        {
            return $"{Type}:{Command}:{IndexCommand}:{IsSynonym}:{TargetFile}:{FileHash:X8}:{BlockOffset}:{BlockNumber}";
        }
    }
}