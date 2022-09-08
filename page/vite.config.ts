import { defineConfig, loadEnv } from 'vite'
import eslintPlugin from 'vite-plugin-eslint'
import vue from '@vitejs/plugin-vue'

// https://vitejs.dev/config/
import { resolve } from 'path'
// @ts-ignore
export default ({ mode }) => {
  return defineConfig({
    plugins: [vue(),
    // 添加下面这块
      eslintPlugin()],
    resolve: {
    // 配置路径别名
      alias: {
        '@': resolve(__dirname, 'src')
      }
    },
    // 配置前端服务地址和端口
    server: {
      host: '0.0.0.0',
      port: 8991,
      // 是否开启 https
      https: false,
      proxy: {
        '/api': {
          target: loadEnv(mode, process.cwd()).VITE_SERVER_URL,
          ws: true,
          changeOrigin: true,
          rewrite: (path) => path.replace(/^\/api/, '')
        }
      }
    }
  })
}
