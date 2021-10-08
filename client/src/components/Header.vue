<template>
  <n-layout-header bordered="" style="padding: .3rem 0 .3rem 0;">
    <n-grid cols="12">
      <n-gi span="2">
        <div class="nav-logo">
          <n-text>Taledynamic</n-text>
        </div>
      </n-gi>
      <n-gi span="6" offset="1">
        <div class="nav-menu">
          <n-menu :value="$route.name" :options="menuOptions" mode="horizontal" />
        </div>
      </n-gi>
      <n-gi span="2" offset="1">
        <div class="nav-extra">
          <n-button @click="changeTheme" text style="margin-right: 1rem">
            <n-icon v-if="currentTheme === null" size="23">
              <svg xmlns="http://www.w3.org/2000/svg" xmlns:xlink="http://www.w3.org/1999/xlink" viewBox="0 0 24 24"><g fill="none"><path d="M20.026 17.001c-2.762 4.784-8.879 6.423-13.663 3.661a9.964 9.964 0 0 1-3.234-2.983a.75.75 0 0 1 .365-1.131c3.767-1.348 5.785-2.911 6.956-5.146c1.232-2.353 1.551-4.93.689-8.464a.75.75 0 0 1 .769-.926a9.961 9.961 0 0 1 4.457 1.327C21.149 6.1 22.788 12.217 20.025 17zm-8.248-4.903c-1.25 2.388-3.31 4.099-6.817 5.499a8.492 8.492 0 0 0 2.152 1.766a8.501 8.501 0 1 0 8.502-14.725a8.485 8.485 0 0 0-2.792-1.016c.647 3.384.23 6.044-1.045 8.476z" fill="currentColor"></path></g></svg>
            </n-icon>
            <n-icon v-else size="25">
              <svg xmlns="http://www.w3.org/2000/svg" xmlns:xlink="http://www.w3.org/1999/xlink" viewBox="0 0 24 24"><g fill="none"><path d="M11.996 19.01a.75.75 0 0 1 .743.649l.007.102v1.5a.75.75 0 0 1-1.493.101l-.007-.101v-1.5a.75.75 0 0 1 .75-.75zm6.022-2.072l1.06 1.06a.75.75 0 1 1-1.06 1.061l-1.06-1.06a.75.75 0 0 1 1.06-1.061zm-10.983 0a.75.75 0 0 1 0 1.06L5.974 19.06a.75.75 0 0 1-1.06-1.06l1.06-1.061a.75.75 0 0 1 1.06 0zM12 6.475a5.525 5.525 0 1 1 0 11.05a5.525 5.525 0 0 1 0-11.05zm0 1.5a4.025 4.025 0 1 0 0 8.05a4.025 4.025 0 0 0 0-8.05zm9.25 3.293a.75.75 0 0 1 .102 1.493l-.102.007h-1.5a.75.75 0 0 1-.102-1.493l.102-.007h1.5zm-17-.029a.75.75 0 0 1 .102 1.494l-.102.006h-1.5a.75.75 0 0 1-.102-1.493l.102-.007h1.5zm1.64-6.37l.084.072l1.06 1.06a.75.75 0 0 1-.976 1.134l-.084-.073l-1.06-1.06a.75.75 0 0 1 .976-1.134zm13.188.072a.75.75 0 0 1 .073.977l-.073.084l-1.06 1.06a.75.75 0 0 1-1.133-.976l.072-.084l1.06-1.061a.75.75 0 0 1 1.061 0zM12 1.99a.75.75 0 0 1 .743.648l.007.102v1.5a.75.75 0 0 1-1.493.101l-.007-.102v-1.5a.75.75 0 0 1 .75-.75z" fill="currentColor"></path></g></svg>
            </n-icon>
          </n-button>
          <n-button text tag="a" href="https://github.com/TaledynamicTeam/Taledynamic" target="_blank">
            <n-icon size="22">
              <svg xmlns="http://www.w3.org/2000/svg" xmlns:xlink="http://www.w3.org/1999/xlink" viewBox="0 0 16 16"><g fill="none"><path d="M8 0C3.58 0 0 3.58 0 8c0 3.54 2.29 6.53 5.47 7.59.4.07.55-.17.55-.38 0-.19-.01-.82-.01-1.49-2.01.37-2.53-.49-2.69-.94-.09-.23-.48-.94-.82-1.13-.28-.15-.68-.52-.01-.53.63-.01 1.08.58 1.23.82.72 1.21 1.87.87 2.33.66.07-.52.28-.87.51-1.07-1.78-.2-3.64-.89-3.64-3.95 0-.87.31-1.59.82-2.15-.08-.2-.36-1.02.08-2.12 0 0 .67-.21 2.2.82.64-.18 1.32-.27 2-.27.68 0 1.36.09 2 .27 1.53-1.04 2.2-.82 2.2-.82.44 1.1.16 1.92.08 2.12.51.56.82 1.27.82 2.15 0 3.07-1.87 3.75-3.65 3.95.29.25.54.73.54 1.48 0 1.07-.01 1.93-.01 2.2 0 .21.15.46.55.38A8.013 8.013 0 0016 8c0-4.42-3.58-8-8-8z" fill-rule="evenodd" fill="currentColor"></path></g></svg> 
            </n-icon>
          </n-button>
        </div>
      </n-gi>
    </n-grid>
    <div>
    </div>
  </n-layout-header>
</template>

<style lang="scss" scoped>
%nav {
  min-width: 100%;
  min-height: 100%;
  display: flex;
  align-items: center;
}

.nav-logo {
  @extend %nav;
  font-size: 1.25rem;
  justify-content: center;
}

.nav-menu {
  @extend %nav;
  justify-content: flex-start;
}

.nav-extra {
  @extend %nav;
  justify-content: center;
}
</style>

<script>
import { h }  from 'vue'
import { RouterLink } from 'vue-router'
import { darkTheme } from 'naive-ui'

const menuOptions = [
  {
    label: () =>
      h(
        RouterLink,
        {
          to: '/'
        },
        { default: () => 'О проекте' }
      ),
    key: 'Home'
  },
  {
    label: () =>
      h(
        RouterLink,
        {
          to: 'auth'
        },
        { default: () => 'Вход' }
      ),
    key: 'Auth'
  }
]

export default {
  data() {
    return {
      menuOptions: menuOptions
    }
  },
  props: {
    currentTheme: Object
  },
  methods: {
    changeTheme() {
      if (this.currentTheme === null) {
        this.$emit('changeTheme', darkTheme)
      } else {
        this.$emit('changeTheme', null)
      }
    }
  }
}
</script>