<template>
  <el-tabs
      v-model="activeName"
      type="card"
      class="tabs"
      @tab-click="changeTab"
  >
    <el-tab-pane label="任务管理" name="first" lazy>
      <task-manager/>
    </el-tab-pane>
    <el-tab-pane label="任务日志管理" name="second" lazy>
      <task-log-manager/>
    </el-tab-pane>
  </el-tabs>
</template>

<script lang="ts" setup>
import { ref } from 'vue'
import type { TabsPaneContext } from 'element-plus'
import TaskLogManager from '@/views/AdminViews/TaskLogManager.vue'
import TaskManager from '@/views/AdminViews/TaskManager.vue';

const activeName = ref('first')

const changeTab = (tab: TabsPaneContext, event: Event) => {

}
</script>

<style scoped>
.tabs{
  margin: 20px 20px 0 20px;
}
</style>
