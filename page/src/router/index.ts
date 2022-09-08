import {
  createRouter,
  createWebHashHistory
} from 'vue-router'

const routes = [{
  path: '/',
  name: 'Home',
  meta: {
    title: '主页'
  },
  // component: HelloWorld
  component: () => import('../views/Admin.vue')
}
]

const router = createRouter({
  history: createWebHashHistory(),
  routes
})
export default router
