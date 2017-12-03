using System.Collections.Generic;
using Shockky.IO;
using Shockky.Shockwave.Chunks;
using Shockky.Shockwave.Chunks.Enum;

namespace Shockky.Shockwave.Lingo
{
    public class LingoScript
    {
        public List<LingoHandler> Handlers { get; }

        public List<string> Properties { get; }
        public List<LingoLiteral> Literals { get; }
        public List<string> Globals { get; }

        public List<string> Names { get; }

        private ShockwaveReader _input;

        public LingoScript(ScriptChunk chunk, ShockwaveReader input) 
        {
            _input = input;
            Names = chunk.NameList;

            var literalsData = chunk[ScriptEntryType.LiteralsData];
            var properties = chunk[ScriptEntryType.Properties];
            var literals = chunk[ScriptEntryType.Literals];
            var handlers = chunk[ScriptEntryType.Handlers];
            var globals = chunk[ScriptEntryType.Globals];

            Properties = input.MapNameList(properties.Length, properties.Offset, Names);
            Globals = input.MapNameList(globals.Length, globals.Offset, Names);

            input.Position = literals.Offset;
            Literals = new List<LingoLiteral>(literals.Length);
            for (int i = 0; i < literals.Length; i++)
            {
                Literals.Add(new LingoLiteral(input, literalsData.Offset));
            }

            input.Position = handlers.Offset;
            Handlers = new List<LingoHandler>(handlers.Length);

            for (int i = 0; i < handlers.Length; i++)
            {
                Handlers.Add(new LingoHandler(this, ref input));
            }

            foreach (var handler in Handlers)
            {
                handler.LoadInstructions();
            }
        }
    }
}
