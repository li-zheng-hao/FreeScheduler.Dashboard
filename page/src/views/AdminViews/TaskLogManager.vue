<template>
  <div class="topBar">
    <el-input
        style="width: 20%"
        v-model="searchText"
        placeholder="请输入任务编号搜索..."
        class="input-with-select"
    >
      <template #prepend>
        <el-button icon="Search" @click="searchClick"/>
      </template>
    </el-input>
    <div style="margin-left: 10px;margin-right: 10px;display: flex;align-items: center;font-size: 14px"> 运行结果:
    </div>
    <el-select v-model="success" placeholder="状态">
      <el-option
          key="-1"
          label="所有"
          :value="-1"
      />
      <el-option
          key="1"
          label="成功"
          :value="1"
      />
      <el-option
          key="2"
          label="失败"
          :value="2"
      />
    </el-select>
  </div>
  <div class="middle">
    <el-table :data="tableData" v-loading="loading" style="width: 100%" :header-cell-style="{'text-align':'center'}"
              empty-text="暂无数据"
              :cell-style="{'text-align':'center'}">
      <el-table-column prop="task_id" label="任务编号"/>
      <el-table-column prop="round" label="运行次数"/>
      <el-table-column prop="elapsed_milliseconds" label="运行时间(毫秒)"/>
      <el-table-column prop="success" label="运行结果"/>
      <el-table-column prop="exception" label="异常"/>
      <el-table-column prop="remark" label="备注"/>
      <el-table-column prop="create_time" label="创建时间"/>
    </el-table>
  </div>
  <div class="bottomBar">
    <el-pagination background layout="prev, pager, next"
                   @current-change="handleCurrentChange" v-model:page-size="pageSize" v-model:current-page="pageIndex"
                   :total="pageCount"/>
  </div>
</template>
<script lang="ts" setup>
import { onMounted, ref } from 'vue'
import { ElMessage } from 'element-plus'
// import { Search, Plus, Edit, Download, Delete } from '@element-plus/icons-vue'
import { Material } from '@/views/AdminViews/types'
import _ from 'lodash';
import FileSaver from 'file-saver';
import { getTaskLogPageList } from '@/request/TaskService';
import { convertUTCDateToLocalDate } from '@/utils/StringUtil';
// region 数据
const searchText = ref<string>()
const tableData = ref<Array<any>>([])
const loading = ref<Boolean>(false)
const pageCount = ref<number>(0)
const pageIndex = ref<number>(1)
const pageSize = ref<number>(10)
const success = ref(-1)
// endregion
const searchClick = async (needOrder: boolean = false) => {
  console.log('点击了搜索');
  loading.value = true
  try {
    const res = await getTaskLogPageList({
      filterKey: searchText.value || '',
      pageNumber: pageIndex.value - 1,
      pageSize: pageSize.value,
      success: success.value
    })
    console.log(res, 'lzh')
    res.rows.forEach(function (item: any) {
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

const handleCurrentChange = (val: number) => {
  searchClick();
}
onMounted(() => {
  searchClick()
})
</script>
<style lang="less">
.topBar {
  display: flex;

}

.middle {
  .el-icon:not(:first-child) {
    margin-left: 5px;
  }

  .el-table {
    margin: 10px 0 10px 0;
    min-height: 50vh;
  }
}

.bottomBar {
  display: flex;
  justify-content: end;
}
</style>
