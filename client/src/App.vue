<template>
  <n-config-provider :theme="theme">
    <n-global-style />
    <n-layout>
      <Header />
      <n-layout-content>
        <router-view />
      </n-layout-content>
    </n-layout>
  </n-config-provider>
</template>

<style lang="scss">
#app {
  font-family: Avenir, Helvetica, Arial, sans-serif;
  -webkit-font-smoothing: antialiased;
  -moz-osx-font-smoothing: grayscale;
  text-align: center;
  color: #2c3e50;
  min-height: 100vh;
}
</style>


<script>
import { darkTheme } from 'naive-ui'
import Header from '@/components/Header.vue'


export default {
  data() {
    return {
      theme: darkTheme
    }
  },
  components: {
    Header
  }
}
</script>