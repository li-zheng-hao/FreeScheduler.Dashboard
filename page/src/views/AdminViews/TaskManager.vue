<template>
  <div class="topBar">
      <el-input
          style="width: 20%"
          v-model="searchText"
          placeholder="请输入主题搜索..."
          class="input-with-select"
      >
        <template #prepend>
          <el-button icon="Search" @click="searchClick"/>
        </template>
      </el-input>

  </div>
  <div class="middle">
    <el-table :data="tableData" v-loading="loading"   style="width: 100%" :header-cell-style="{'text-align':'center'}"
              empty-text="暂无数据"
              :cell-style="{'text-align':'center'}">
      <el-table-column prop="isEnabled" label="是否启用" width="100">
        <template #default="scope">
          <el-switch :disabled="scope.row.status==='Completed'" v-model="scope.row.isEnabled" @change="enableChanged(scope.row)"/>
        </template>
      </el-table-column>
      <el-table-column prop="id" label="任务编号"   />
      <el-table-column prop="topic" label="主题"  />
      <el-table-column prop="body" label="消息体"  />
      <el-table-column prop="round" label="运行次数"  />
      <el-table-column prop="interval" label="间隔(单位)"  />
      <el-table-column prop="interval_argument" label="间隔"  />
      <el-table-column prop="create_time" label="创建时间"  />
      <el-table-column prop="last_run_time" label="最后运行时间"  />
      <el-table-column prop="current_round" label="当前运行次数"  />
      <el-table-column prop="error_times" label="错误次数"  />
      <el-table-column prop="status" label="状态"  />
    </el-table>
  </div>
  <div class="bottomBar">  <el-pagination background layout="prev, pager, next"
                                          @current-change="handleCurrentChange" v-model:page-size="pageSize" v-model:current-page="pageIndex"  :total="pageCount" /></div>
</template>
<script lang="ts" setup>
import { onMounted, ref } from 'vue'
import { ElMessage } from 'element-plus'
// import { Search, Plus, Edit, Download, Delete } from '@element-plus/icons-vue'
import { Material } from '@/views/AdminViews/types'
import _ from 'lodash';
import FileSaver from 'file-saver';
import { getTaskPageList, updateTaskState } from '@/request/TaskService';
import { convertUTCDateToLocalDate } from '@/utils/StringUtil';
// region 数据
const searchText = ref<string>()
const tableData = ref<Array<any>>([])
const loading = ref<Boolean>(false)
const pageCount = ref<number>(0)
const pageIndex = ref<number>(1)
const pageSize = ref<number>(10)
const orderColumnName = ref('')
const orderType = ref(0)
// endregion
const searchClick = async (needOrder:boolean = false) => {
  console.log('点击了搜索');
  loading.value = true
  try {
    const res = await getTaskPageList({ filterKey: searchText.value || '', pageNumber: pageIndex.value - 1, pageSize: pageSize.value })
    console.log(res, 'lzh')
    res.rows.forEach(function (item:any) {
      item.create_time = convertUTCDateToLocalDate(new Date(item.create_time)).toLocaleString();
      item.last_run_time = convertUTCDateToLocalDate(new Date(item.last_run_time)).toLocaleString();
      item.isEnabled = item.status === 'Running';
    });
    console.log(res.rows)
    tableData.value = res.rows
    pageCount.value = res.count
    ElMessage.success('查询完成')
  } finally {
    loading.value = false
  }
}
const enableChanged = (row:any) => {
  row.status = row.isEnabled ? 'Running' : 'Paused'
  updateTaskState({ task_id: row.id, status: row.isEnabled ? 0 : 1 })
  ElMessage.success('点击了切换')
}
const handleCurrentChange = (val: number) => {
  searchClick();
}
onMounted(() => {
  searchClick()
})
</script>
<style lang="less">
.topBar{
  display: flex;

}
.middle{
  .el-icon:not(:first-child){
    margin-left: 5px;
  }
  .el-table{
    margin: 10px 0 10px 0;
    min-height: 50vh;
  }
}

.bottomBar{
  display: flex;
  justify-content: end;
}
</style>
