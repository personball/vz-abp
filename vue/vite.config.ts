/// <reference types="vitest" />

import path from 'path'
import { defineConfig } from 'vite'
import vue from '@vitejs/plugin-vue'
import AutoImport from 'unplugin-auto-import/vite'
import VueRouter from 'unplugin-vue-router/vite'
import { VueRouterAutoImports } from 'unplugin-vue-router'
import Components from 'unplugin-vue-components/vite'
import { ElementPlusResolver } from 'unplugin-vue-components/resolvers'
import Layouts from 'vite-plugin-vue-layouts';

import Unocss from 'unocss/vite'
import {
  presetAttributify,
  presetIcons,
  presetUno,
  transformerDirectives,
  transformerVariantGroup,
} from 'unocss'

const pathSrc = path.resolve(__dirname, 'src')

// https://vitejs.dev/config/
export default defineConfig({
  resolve: {
    alias: {
      '~/': `${pathSrc}/`,
    },
  },
  css: {
    preprocessorOptions: {
      scss: {
        additionalData: `@use "~/styles/element/index.scss" as *;`,
      },
    },
  },
  plugins: [
    VueRouter({
      routesFolder: 'src/routes',
      extensions: ['.vue'],
      exclude: ['**/__*'],
      dts: 'src/typed-router.d.ts',
      importMode: 'async'
    }),
    vue(),
    Components({
      // allow auto load markdown components under `./src/components/`
      extensions: ['vue', 'md'],
      // allow auto import and register components used in markdown
      include: [/\.vue$/, /\.vue\?vue/, /\.md$/],
      resolvers: [
        ElementPlusResolver({
          importStyle: 'sass',
        }),
      ],
      // dirs:['src/components','src/views'],// only global components
      dts: 'src/components.d.ts',
      types: [{
        from: 'vue-router',
        names: ['RouterLink', 'RouterView']
      }]
    }),

    // https://github.com/antfu/unocss
    // see unocss.config.ts for config
    Unocss({
      presets: [
        presetUno(),
        presetAttributify(),
        presetIcons({
          scale: 1.2,
          warn: true,
        }),
      ],
      transformers: [
        transformerDirectives(),
        transformerVariantGroup(),
      ]
    }),
    // https://github.com/antfu/unplugin-auto-import
    AutoImport({
      // targets to transform
      include: [
        /\.[tj]sx?$/, // .ts, .tsx, .js, .jsx
        /\.vue$/, /\.vue\?vue/, // .vue
        /\.md$/, // .md
      ],

      // global imports to register
      imports: [
        // presets
        'vue',
        VueRouterAutoImports,
        {
          'vue-router/auto': ['useLink'],
          'vue3-oidc': ['useOidcStore', 'useAuth']
        }
        // custom
        // {
        //   '@vueuse/core': [
        //     // named imports
        //     'useMouse', // import { useMouse } from '@vueuse/core',
        //     // alias
        //     ['useFetch', 'useMyFetch'], // import { useFetch as useMyFetch } from '@vueuse/core',
        //   ],
        //   'axios': [
        //     // default imports
        //     ['default', 'axios'], // import { default as axios } from 'axios',
        //   ],
        //   '[package-name]': [
        //     '[import-names]',
        //     // alias
        //     ['[from]', '[alias]'],
        //   ],
        // },
        // example type import
        // {
        //   from: 'vue-router',
        //   imports: ['RouteLocationRaw'],
        //   type: true,
        // },
      ],
      // Enable auto import by filename for default module exports under directories
      defaultExportByFilename: false,

      // Auto import for module exports under directories
      // by default it only scan one level of modules under the directory
      dirs: [
        // './hooks',
        // './composables' // only root modules
        'src/composables/**', // all nested modules
        // ...
      ],

      // Filepath to generate corresponding .d.ts file.
      // Defaults to './auto-imports.d.ts' when `typescript` is installed locally.
      // Set `false` to disable.
      dts: 'src/auto-imports.d.ts',

      // Auto import inside Vue template
      // see https://github.com/unjs/unimport/pull/15 and https://github.com/unjs/unimport/pull/72
      vueTemplate: false,

      // Custom resolvers, compatible with `unplugin-vue-components`
      // see https://github.com/antfu/unplugin-auto-import/pull/23/
      resolvers: [
        /* ... */
      ],

      // Inject the imports at the end of other imports
      injectAtEnd: true,

      // Generate corresponding .eslintrc-auto-import.json file.
      // eslint globals Docs - https://eslint.org/docs/user-guide/configuring/language-options#specifying-globals
      eslintrc: {
        enabled: false, // Default `false`
        filepath: './.eslintrc-auto-import.json', // Default `./.eslintrc-auto-import.json`
        globalsPropValue: true, // Default `true`, (true | false | 'readonly' | 'readable' | 'writable' | 'writeable')
      },
    }),
    Layouts()
  ],
  // https://cn.vitest.dev/config/
  test: {
    globals: true,
    environment: 'happy-dom'
  }
})
