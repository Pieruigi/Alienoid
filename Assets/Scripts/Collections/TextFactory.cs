using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Zom.Pie.Collections
{
    
    public class TextFactory
    {
        public enum Type { UIMessage, UILabel }

        public static readonly string ResourceFolder = "Texts";

        Dictionary<Type, TextCollection> messages;

        static TextFactory instance;
        public static TextFactory Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new TextFactory();
                }

                return instance;
            }
        }

        protected TextFactory()
        {
            messages = new Dictionary<Type, TextCollection>();

            // Load text resources depending on the language and the file name
            //string folder = System.IO.Path.Combine(ResourceFolder, GameManager.Instance.Language.ToString());
            //string folder = System.IO.Path.Combine(ResourceFolder, GameManager.Instance.Language.ToString());

            // Load UIMessages
            string path = System.IO.Path.Combine(ResourceFolder, GetFileName(Type.UIMessage));
            TextCollection collection = Resources.Load<TextCollection>(path);
            messages.Add(Type.UIMessage, collection);

            // Load UILabels
            path = System.IO.Path.Combine(ResourceFolder, GetFileName(Type.UILabel));
            collection = Resources.Load<TextCollection>(path);
            messages.Add(Type.UILabel, collection);

        }

        /// <summary>
        /// Returns text by id.
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public string GetText(Type type, int id)
        {
            return messages[type].GetText(id);
        }

        string GetFileName(Type type)
        {
            string ret = "";
            switch (type)
            {
                case Type.UIMessage:
                    ret = "UIMessages";
                    break;
                case Type.UILabel:
                    ret = "UILabels";
                    break;
            }
            
            if(Application.isPlaying)
                ret += "_" + GameManager.Instance.Language.ToString();
            else
                ret += "_" + Constants.DefaultLanguage.ToString();

            return ret;
        }
    }

}
