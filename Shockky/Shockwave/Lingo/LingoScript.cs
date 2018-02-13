using System.Collections.Generic;
using System.Linq;
using Shockky.IO;
using Shockky.Shockwave.Chunks;
using Shockky.Shockwave.Chunks.Enum;

namespace Shockky.Shockwave.Lingo
{
    public class LingoScript
    {
        private readonly ShockwaveReader _input;

        public List<LingoHandler> Handlers { get; }

        public List<LingoLiteral> Literals { get; }
        public List<string> Properties { get; }
        public List<string> Globals { get; }
        public List<string> Names { get; }

        public LingoScript(ScriptChunk chunk, ShockwaveReader input, List<string> nameList)
        {
            List<string> MapScriptEntryList(int len, int offset) =>
                input.ReadBigEndianList<short>(len, offset)
                    .Where(i => i > 0 && i < Names.Count)  //TODO: wazzzuuup
                    .Select(i => Names[i]).ToList();

            _input = input;
            Names = nameList;

            var literalsData = chunk[ScriptEntryType.LiteralsData];
            var properties = chunk[ScriptEntryType.Properties];
            var literals = chunk[ScriptEntryType.Literals];
            var handlers = chunk[ScriptEntryType.Handlers];
            var globals = chunk[ScriptEntryType.Globals];

            Properties = MapScriptEntryList(properties.Length, properties.Offset);
            Globals = MapScriptEntryList(globals.Length, globals.Offset);

            input.Position = literals.Offset;
            Literals = new List<LingoLiteral>(literals.Length);
            for (int i = 0; i < literals.Length; i++)
            {
                Literals.Add(new LingoLiteral(input, literalsData.Offset));
            }

            //AST starts here

            input.Position = handlers.Offset;
            Handlers = new List<LingoHandler>(handlers.Length);

            for (int i = 0; i < handlers.Length; i++)
            {
                Handlers.Add(new LingoHandler(this, input));
            }
        }

	    public void Disassemble()
	    {

	    }
    }
}