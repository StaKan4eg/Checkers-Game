namespace checkers.Properties {
    using System;

    // Класс ресурсов с явно заданным типом, для поиска локализованных строк 
    // класс был автоматически сгенерирован с помощью StronglyTypedResourceBuilder
    [global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Resources.Tools.StronglyTypedResourceBuilder", "17.0.0.0")]
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
    [global::System.Runtime.CompilerServices.CompilerGeneratedAttribute()]
    internal class Resources {
        
        private static global::System.Resources.ResourceManager resourceMan;
        
        private static global::System.Globalization.CultureInfo resourceCulture;
        
        [global::System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode")]
        internal Resources() {
        }

        // Возвращает кэшированный экземпляр ResourceManager, используемый этим классом
        [global::System.ComponentModel.EditorBrowsableAttribute(global::System.ComponentModel.EditorBrowsableState.Advanced)]
        internal static global::System.Resources.ResourceManager ResourceManager {
            get {
                if (object.ReferenceEquals(resourceMan, null)) {
                    global::System.Resources.ResourceManager temp = new global::System.Resources.ResourceManager("checkers.Properties.Resources", typeof(Resources).Assembly);
                    resourceMan = temp;
                }
                return resourceMan;
            }
        }

        // Переопределяет свойство CurrentUICulture текущего потока для всех
        // поисков ресурсов с использованием этого явно заданного класса ресурсов
        [global::System.ComponentModel.EditorBrowsableAttribute(global::System.ComponentModel.EditorBrowsableState.Advanced)]
        internal static global::System.Globalization.CultureInfo Culture {
            get {
                return resourceCulture;
            }
            set {
                resourceCulture = value;
            }
        }

        // Ищем локализованный ресурс типа System.Drawing.Bitmap.
        internal static System.Drawing.Bitmap blackPiece {
            get {
                object obj = ResourceManager.GetObject("blackPiece", resourceCulture);
                return ((System.Drawing.Bitmap)(obj));
            }
        }
        
        internal static System.Drawing.Bitmap blackPieceKing {
            get {
                object obj = ResourceManager.GetObject("blackPieceKing", resourceCulture);
                return ((System.Drawing.Bitmap)(obj));
            }
        }
        
        internal static System.Drawing.Bitmap whitePiece {
            get {
                object obj = ResourceManager.GetObject("whitePiece", resourceCulture);
                return ((System.Drawing.Bitmap)(obj));
            }
        }
        
        internal static System.Drawing.Bitmap whitePieceKing {
            get {
                object obj = ResourceManager.GetObject("whitePieceKing", resourceCulture);
                return ((System.Drawing.Bitmap)(obj));
            }
        }
    }
}
