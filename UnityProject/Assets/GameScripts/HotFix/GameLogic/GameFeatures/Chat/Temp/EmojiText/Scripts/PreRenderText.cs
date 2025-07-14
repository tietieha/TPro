using System;
using System.Text.RegularExpressions;

/**
 * 目的：解决对emojiText支持渲染富文本时造成的渲染异常
 */
namespace Plugins.EmojiText.Scripts
{
    public class PreRenderText
    {
        private string originText = String.Empty;
     
        public void SetRenderText(string str)
        {
            originText = str;
        }
        
        //预渲染的文本
        public string GetPreRenderText(bool supportRichText)
        {
            //因为空格不会被渲染 所以过滤空格
            var result = Regex.Replace(originText, @"\s", "");
            if(supportRichText)
                result = RichLabelUtils.RemoveLabels(result);
            return result;
        }
        
        public void ReplaceFirst(string search, string replace)
        {
            int pos = originText.IndexOf(search, StringComparison.Ordinal);
            if (pos < 0)
            {
                return ;
            }
            originText = originText.Substring(0, pos) + replace + originText.Substring(pos + search.Length);
        }
    }
}