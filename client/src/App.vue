<template>
  <n-config-provider :theme="currentTheme">
    <n-global-style />
    <n-layout>
      <Header @changeTheme="setTheme" :currentTheme="currentTheme" />
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
import { darkTheme, useOsTheme } from 'naive-ui'
import Header from '@/components/Header.vue'

export default {
  data() {
    return {
      currentTheme: darkTheme
    }
  },
  components: {
    Header
  },
  methods: {
    setTheme(value) {
      this.currentTheme = value;
      this.$cookie.setCookie('theme', value === null ? 'light' : 'dark');
    }
  },
  mounted() {
    const cookieTheme = this.$cookie.getCookie('theme');

    if (cookieTheme === null) {
      const osThemeRef = useOsTheme();
      this.currentTheme = (osThemeRef.value === 'dark' ? darkTheme : null);
    } else {
      this.currentTheme = (cookieTheme === 'dark' ? darkTheme : null);
    }
  }
}
</script>