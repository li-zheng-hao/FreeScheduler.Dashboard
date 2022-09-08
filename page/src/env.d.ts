/// <reference types="vite/client" />

interface ImportMetaEnv {
    // 环境变量名称
    readonly VITE_NAME: string
    // 更多环境变量...
}

// eslint-disable-next-line no-unused-vars
interface ImportMeta {
    readonly env: ImportMetaEnv
}