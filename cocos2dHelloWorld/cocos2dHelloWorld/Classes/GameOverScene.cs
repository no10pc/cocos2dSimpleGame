using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using cocos2d;
namespace cocos2dSimpleGame.Classes
{
    class GameOverScene : CCScene
    {
        public CCLabelTTF label;
        public GameOverScene()
        {
        }
        public GameOverScene(string msg)
        {
            CCLayerColor colorlayer = CCLayerColor.layerWithColor(new ccColor4B(255, 255, 255, 255));
            this.addChild(colorlayer);
            CCSize winSize = CCDirector.sharedDirector().getWinSize();
            label = CCLabelTTF.labelWithString(msg, "Arial", 32);
            label.Color = new ccColor3B(0, 0, 0);
            label.position = new CCPoint(winSize.width / 2, winSize.height / 2);
            this.addChild(label);
            this.runAction(CCSequence.actions(
                CCDelayTime.actionWithDuration(3), CCCallFunc.actionWithTarget(this, gameOverDone)));
        }
        void gameOverDone()
        {
            CCScene pScene = CCScene.node();
            pScene.addChild(MainMenu.node());
            CCDirector.sharedDirector().replaceScene(pScene);
        }
    }
}
