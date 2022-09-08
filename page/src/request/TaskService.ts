// 导入axios实例
import httpRequest from '@/request/index'
import _ from 'lodash'

// 分页获取胚料信息
export async function getTaskPageList (data:any) {
  console.log(data, 'data')
  const res = await httpRequest({
    url: `scheduler/Task/PageQueryTask?pageNumber=${data.pageNumber}&pageSize=${data.pageSize}&filterKey=${data.filterKey}`,
    method: 'get'
  });
  console.log(res, 'gettaskpagelist');
  // @ts-ignore
  return { rows: res.rows, count: res.count };
}

// 更新状态
export async function updateTaskState (data:any) {
  const res = await httpRequest({
    url: 'scheduler/Task/UpdateTaskState',
    method: 'post',
    data
  });
}
export async function getTaskLogPageList (data:any) {
  console.log(data, 'data')
  const res = await httpRequest({
    url: `scheduler/Task/PageQueryTaskLog?pageNumber=${data.pageNumber}&pageSize=${data.pageSize}&filterKey=${data.filterKey}&success=${data.success}`,
    method: 'get'
  });
  console.log(res, 'gettasklogpagelist')
  // @ts-ignore
  return { rows: res.rows, count: res.count };
}
