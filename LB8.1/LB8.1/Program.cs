using System;
using System.IO; // StreamReader
using OpenTK;
using OpenTK.Graphics.OpenGL;
using OpenTK.Input; // KeyboardKeyEventArgs

namespace SimpleShader
{
    public class Simple_shader : GameWindow
    {
        public Simple_shader() : base(300, 300)
        {
            KeyDown += Keyboard_KeyDown; // Обработчик нажатий на клавиши клавиатуры
        }
        // Шейдеры вершин и фрагментов
        int VertexShaderObject, FragmentShaderObject, ProgramObject;
        //const string VertexShaderFilename = "blinnVS.glsl";
        //const string FragmentShaderFilename = "blinnFS.glsl";
        const string VertexShaderFilename = "phongVS.glsl";
        const string FragmentShaderFilename = "phongFS.glsl";
        Vector3 EyePos = new Vector3(0f, 0f, 6f); // Камера
        Vector3 LightPos = new Vector3(0f, 0f, 2f); // Источник света
        float specPower = 14; // Контрастность блика

        // Обработчик нажатий на клавиши клавиатуры
        void Keyboard_KeyDown(object sender, KeyboardKeyEventArgs e)
        {
            if (e.Key == Key.Escape) this.Exit();
            if (e.Key == Key.F11) specPower += 2;
            if (e.Key == Key.F12) if (specPower > 2) specPower -= 2;
        }
        private int readShader(string shaderFilename, ShaderType st, string vf)
        {
            string LogInfo;
            int shaderObject = GL.CreateShader(st);
            using (StreamReader sr = new StreamReader(shaderFilename))
            {
                GL.ShaderSource(shaderObject, sr.ReadToEnd());
                GL.CompileShader(shaderObject);
            }
            GL.GetShaderInfoLog(shaderObject, out LogInfo);
            if (LogInfo.Length > 0 && !LogInfo.Contains("hardware"))
                Console.WriteLine("Ошибка компиляции шейдера " + vf + "\nLog:\n" + LogInfo);
            else
                Console.WriteLine("Компиляция шейдера " + vf + " завершена успешно.");
            return shaderObject;
        }
        protected override void OnLoad(EventArgs e)
        {
            string LogInfo;
            int[] temp = new int[1];
            GL.ClearColor(0.2f, 0f, 0.4f, 0f); // Цвет фона
            // Загрузка и компиляция шейдеров вершин и фрагментов
            VertexShaderObject = readShader(VertexShaderFilename, ShaderType.VertexShader, "вершин");
            FragmentShaderObject = readShader(FragmentShaderFilename, ShaderType.FragmentShader, "фрагментов");
            // Связываем шейдеры с рабочей программой
            ProgramObject = GL.CreateProgram();
            GL.AttachShader(ProgramObject, VertexShaderObject);
            GL.AttachShader(ProgramObject, FragmentShaderObject);
            // Связываем все вместе
            GL.LinkProgram(ProgramObject);
            // Удаляем ранее созданные шейдеры
            GL.DeleteShader(VertexShaderObject);
            GL.DeleteShader(FragmentShaderObject);
            GL.GetProgram(ProgramObject, GetProgramParameterName.LinkStatus, out temp[0]);
            Console.WriteLine("Связывание программ (" + ProgramObject + ") " + ((temp[0] == 1) ? "выполнено." : "НЕ ВЫПОЛНЕНО."));
            if (temp[0] != 1) // В случае неудачи при связывании
            {
                GL.GetProgramInfoLog(ProgramObject, out LogInfo);
                Console.WriteLine("Информация:\n" + LogInfo);
            }
            GL.GetProgram(ProgramObject, GetProgramParameterName.ActiveAttributes, out temp[0]);
            Console.WriteLine("Зарегестрировано " + temp[0] + " атрибута.");
            Console.WriteLine("Создание шейдера завершено. GL-ошибка: " + GL.GetError() + ".");
            Console.WriteLine("");
        }
        protected override void OnUnload(EventArgs e)
        {
            GL.DeleteProgram(ProgramObject);
            base.OnUnload(e);
        }
        // Обработчик изменения размеров окна графического вывода
        protected override void OnResize(EventArgs e)
        {
            GL.Viewport(0, 0, Width, Height);
            GL.MatrixMode(MatrixMode.Projection);
            Matrix4 p = Matrix4.CreatePerspectiveFieldOfView(MathHelper.PiOver4, Width / (float)Height, 0.1f, 100.0f);
            GL.LoadMatrix(ref p);
            GL.MatrixMode(MatrixMode.Modelview);
            GL.LoadIdentity();
            base.OnResize(e);
        }
        // Обработчик обновления
        protected override void OnUpdateFrame(FrameEventArgs e)
        {
            base.OnUpdateFrame(e);
        }
        // Вывод сферы, формирование координат нормалей и текстуры
        // r - радиус сферы
        // nx - число полигонов (сегментов) по X
        // ny - число полигонов (сегментов) по Y
        void sphere(double r, int nx, int ny)
        {
            int ix, iy;
            double rsy, rcy, rsy1, rcy1, sx, cx, piy, pix, ay, ay1, ax;
            Vector3d vert;
            piy = Math.PI / (double)ny;
            pix = 2.0 * Math.PI / (double)nx;
            // Рисуем полигональную модель сферы и формируем нормали
            // Каждый полигон - это трапеция. Трапеции верхнего и нижнего слоев вырождаются в треугольники
            GL.Begin(PrimitiveType.QuadStrip);
            ay = -piy;
            for (iy = 0; iy < ny; iy++)
            {
                ay += piy;
                rsy = r * Math.Sin(ay);
                rcy = r * Math.Cos(ay);
                ay1 = ay + piy;
                rsy1 = r * Math.Sin(ay1);
                rcy1 = r * Math.Cos(ay1);
                ax = pix;
                for (ix = 0; ix <= nx; ix++)
                {
                    ax -= pix;
                    sx = Math.Sin(ax);
                    cx = Math.Cos(ax);
                    vert = new Vector3d(rsy * cx, rsy * sx, -rcy);
                    GL.Normal3(vert); // Координаты нормали в текущей вершине; нормаль направлена от центра
                    GL.Vertex3(vert);
                    vert = new Vector3d(rsy1 * cx, rsy1 * sx, -rcy1);
                    GL.Normal3(vert);
                    GL.Vertex3(vert);
                }
            }
            GL.End();
        }
        // Обработчик воспроизведения
        protected override void OnRenderFrame(FrameEventArgs e)
        {
            GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);
            GL.UseProgram(ProgramObject);
            GL.Uniform3(GL.GetUniformLocation(ProgramObject, "EyePos"), EyePos.X, EyePos.Y, EyePos.Z);
            GL.Uniform3(GL.GetUniformLocation(ProgramObject, "LightPos"), LightPos.X, LightPos.Y, LightPos.Z);
            GL.Uniform1(GL.GetUniformLocation(ProgramObject, "specPower"), specPower);
            GL.PushMatrix();
            Matrix4 t = Matrix4.LookAt(EyePos, Vector3.Zero, Vector3.UnitY);
            GL.MultMatrix(ref t);
            sphere(1, 32, 32); // Выводим сферу
            EyePos = new Vector3(0f, 0f, 5f); // Камера
            GL.Uniform3(GL.GetUniformLocation(ProgramObject, "EyePos"), EyePos.X, EyePos.Y, EyePos.Z);
            t = Matrix4.LookAt(EyePos, Vector3.Zero, Vector3.Zero);
            GL.MultMatrix(ref t);
            sphere(1, 32, 32);
            GL.UseProgram(0);
            GL.PopMatrix();
            this.SwapBuffers();
        }
        // Точка входа
        [STAThread]
        public static void Main()
        {
            using (Simple_shader mapping = new Simple_shader())
            {
                mapping.Title = "Blinn/Phong shader";
                mapping.Run(10.0, 2.0);
            }
        }
    }
}