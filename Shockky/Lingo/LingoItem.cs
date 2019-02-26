using System;

using Shockky.Chunks;

namespace Shockky.Lingo
{
    public abstract class LingoItem : ShockwaveItem
    {
        public ScriptChunk Script { get; }

        protected LingoItem(ScriptChunk script)
        {
            Script = script;
        }
        
        public virtual string ToLingo()
        {
            throw new NotSupportedException();
        }
    }
}