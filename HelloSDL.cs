using System;
using static SDL2.SDL;

namespace SDLCSharpDemo
{
    internal class HelloSDL
    {
        enum LineFillMode {
            None,

            /// <summary>
            /// border
            /// </summary>
            B,

            /// <summary>
            /// border + fill
            /// </summary>
            BF
        }


        bool running = true;

        IntPtr window, renderer;

        void SetColour(int rgb) {

            var r = (byte)(rgb / 0x10000);
            var g = (byte)(rgb / 0x100 % 0x100);
            var b = (byte)(rgb % 0x100);

            SDL_SetRenderDrawColor(renderer, r, g, b, 255);
        }

        /// <summary>
        /// Based on QuickBASIC's LINE Statement
        /// </summary>
        /// <param name="colour">RGB value</param>
        void Line(
            int x1, int y1, int x2, int y2,
            int colour,
            LineFillMode border_fill = LineFillMode.None)
        {  // , B Or BF, style%

            byte r, g, b, a;
            SDL_GetRenderDrawColor(renderer, out r, out g, out b, out a);

            SetColour(colour);

            SDL_Rect rect = new SDL_Rect();

            if (border_fill == LineFillMode.BF || border_fill == LineFillMode.B) {
                int min_x = Math.Min(x1, x2);
                int min_y = Math.Min(y1, y2);
                int max_x = Math.Max(x1, x2);
                int max_y = Math.Max(y1, y2);

                rect = new SDL_Rect() {
                    x = x1,
                    y = y1,
                    w = max_x - min_x,
                    h = max_y - min_y
                };
            }



            switch (border_fill) {
                case LineFillMode.BF:
                    SDL_RenderFillRect(renderer, ref rect);
                    break;

            case LineFillMode.B:
                    SDL_RenderDrawRect(renderer, ref rect);

                    // rect = null;
                    break;

            default:
                    SDL_RenderDrawLine(renderer, x1, y1, x2, y2);
                    break;
        }

            SDL_SetRenderDrawColor(renderer, r, g, b, a);
    }


        void Setup()
        {
            SDL_Init(SDL_INIT_VIDEO);

            window = SDL_CreateWindow(
                "Hello SDL",
                SDL_WINDOWPOS_UNDEFINED, SDL_WINDOWPOS_UNDEFINED,
                320, 240,
                SDL_WindowFlags.SDL_WINDOW_SHOWN);

            renderer = SDL_CreateRenderer(
                window, -1,
                SDL_RendererFlags.SDL_RENDERER_ACCELERATED | SDL_RendererFlags.SDL_RENDERER_PRESENTVSYNC);
        }

        void PollEvents() {
            SDL_Event e;

            while (SDL_PollEvent(out e) == 1)
            {
                switch (e.type) {
                    case SDL_EventType.SDL_QUIT:
                        running = false;
                        break;

                    case SDL_EventType.SDL_KEYDOWN:
                        switch (e.key.keysym.sym) {
                            case SDL_Keycode.SDLK_q:
                                running = false;
                                break;
                        }
                        break;
                }
            }
        }

        void Render() {
            // SDL_SetRenderDrawColor(renderer, 0, 0, 0, 255)
            SetColour(0x6495ED);
            SDL_RenderClear(renderer);

            Line(0, 0, 320, 240, 0xFF0000);

            Line(30, 10, 50, 30, 0x0, LineFillMode.BF);

            SDL_RenderPresent(renderer);
        }


        void CleanUp() {
            SDL_DestroyRenderer(renderer);
            SDL_DestroyWindow(window);
            SDL_Quit();
        }


        public HelloSDL()
        {
            running = true;

            Setup();

            while (running)
            {
                PollEvents();
                    Render();
            }

            CleanUp();
        }

}
}
