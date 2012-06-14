using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using cocos2d;

namespace cocos2dSimpleGame.Classes
{
    class AboutScene : CCScene
    {
        public AboutScene()
        {
            CCLayerColor colorLayer = CCLayerColor.layerWithColor(new ccColor4B(255, 255, 255, 255));
            this.addChild(colorLayer);
            string msg = "此程序由fengyun1989基于子龙山人翻译\r\n的IPHONE教程制作,仅供学习交流之用，\r\n切勿进行商业传播。如有什么意见或者建议，\r\n请发邮件到1024919409@qq.com！";
            CCLabelTTF label = CCLabelTTF.labelWithString(msg, "StartFont", 24);
            CCSize winSize = CCDirector.sharedDirector().getWinSize();
            label.position = new CCPoint(winSize.width / 2, winSize.height / 2 - 40);
            this.addChild(label);
        }

    }  
}
