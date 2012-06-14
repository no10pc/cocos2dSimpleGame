using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content.Pipeline;
using Microsoft.Xna.Framework.Content.Pipeline.Graphics;
using Microsoft.Xna.Framework.Content.Pipeline.Processors;
using System.IO;
using System.ComponentModel;

// TODO: 将这些项替换为处理器输入和输出类型。

namespace FontProcessor
{

    [ContentProcessor(DisplayName = "FontProcessor.ContentProcessor1")]
    public class ContentProcessor1 : FontDescriptionProcessor
    {
        public override SpriteFontContent Process(FontDescription input, ContentProcessorContext context)
        {
            // TODO: 处理输入对象，并返回修改的数据。
            string fullPath = Path.GetFullPath(MessageFile);
            context.AddDependency(fullPath);
            string letters = File.ReadAllText(fullPath, System.Text.Encoding.UTF8);
            foreach (char c in letters)
            {
                input.Characters.Add(c);
            }

            return base.Process(input, context);
        }
        public string MessageFile
        {
            get { return messageFile; }
            set { messageFile = value; }

        }
        private string messageFile = @"..\cocos2dHelloWorld\messages.txt";
        
    }
  
}
