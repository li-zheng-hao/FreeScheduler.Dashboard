
export interface Material {
    id?:string, // id
    imageUrl:string, // 缩略图地址
    name:string, // 名称
    specification?:string, // 规格
    material?:string, // 材质
    modelFileName?:string, // 模型文件名称
    modelFileSize?:string, // 模型文件大小
    referenceLibUrl?:string, // 引用插件的url
    comment?:string // 备注
}
export interface ProcessLib {
    isEnabled:boolean, // 是否启用
    processName:string, // 加工名称
    material:Material, // 缩略图地址
    documentFileType:string, // 说明文件类型
    documentFileName:string, // 说明书文件名称
    documentFileSize:string, // 说明书文件大小
    libName:string, // 插件名称
    libSize:string // 插件大小
}
// 操作类型
export enum OpType {
    Add=0, // 新增
    Modify=1 // 修改
}
