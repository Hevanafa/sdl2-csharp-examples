using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using static SDL2.SDL;
using static SDL2.SDL_image;

namespace SDLCSharpDemo
{
    internal class PNGLoadDemo
    {
        bool running = true;

        IntPtr window, renderer;

        void SetColour(int rgb)
        {

            var r = (byte)(rgb / 0x10000);
            var g = (byte)(rgb / 0x100 % 0x100);
            var b = (byte)(rgb % 0x100);

            SDL_SetRenderDrawColor(renderer, r, g, b, 255);
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


            // Init image loader
            int result = IMG_Init(IMG_InitFlags.IMG_INIT_PNG);

            // TODO: Complete the list of dependencies

            // Complete dependencies:
            // https://stackoverflow.com/questions/21472958

            // IMG_Init ref:
            // https://cpp.hotexamples.com/examples/-/-/IMG_Init/cpp-img_init-function-examples.html
            if ((result & (int)IMG_InitFlags.IMG_INIT_PNG) == 0)
            {
                Console.WriteLine("IMG_Init returned " + result);
                Console.WriteLine(IMG_GetError());
                //Thread.Sleep(6000);
                return;
            }
            
            //img = IMG_Load("Camellia.png");
            texture = IMG_LoadTexture(renderer, "Camellia.png");
        }

        void PollEvents()
        {
            SDL_Event e;

            while (SDL_PollEvent(out e) == 1)
            {
                switch (e.type)
                {
                    case SDL_EventType.SDL_QUIT:
                        running = false;
                        break;

                    case SDL_EventType.SDL_KEYDOWN:
                        switch (e.key.keysym.sym)
                        {
                            case SDL_Keycode.SDLK_q:
                                running = false;
                                break;
                        }
                        break;
                }
            }
        }

        void Render()
        {
            // SDL_SetRenderDrawColor(renderer, 0, 0, 0, 255)
            SetColour(0x6495ED);
            SDL_RenderClear(renderer);

            // Todo: draw the PNG here
            var dest_rect = new SDL_Rect()
            {
                x = 5,
                y = 5,
                w = 100,
                h = 100
            };

            SDL_RenderCopy(renderer, texture, IntPtr.Zero, ref dest_rect);

            SDL_RenderPresent(renderer);
        }


        void CleanUp()
        {
            IMG_Quit();

            SDL_DestroyTexture(texture);
            SDL_FreeSurface(img);

            SDL_DestroyRenderer(renderer);
            SDL_DestroyWindow(window);
            SDL_Quit();
        }

        IntPtr img; // SDL_Surface
        IntPtr texture;

        public PNGLoadDemo()
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
