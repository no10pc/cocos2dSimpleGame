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
            string msg = "�˳�����fengyun1989��������ɽ�˷���\r\n��IPHONE�̳�����,����ѧϰ����֮�ã�\r\n���������ҵ����������ʲô������߽��飬\r\n�뷢�ʼ���1024919409@qq.com��";
            CCLabelTTF label = CCLabelTTF.labelWithString(msg, "StartFont", 24);
            CCSize winSize = CCDirector.sharedDirector().getWinSize();
            label.position = new CCPoint(winSize.width / 2, winSize.height / 2 - 40);
            this.addChild(label);
        }

    }  
}
