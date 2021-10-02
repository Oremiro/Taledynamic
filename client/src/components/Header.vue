<template>
  <n-layout-header bordered="" style="padding: .3rem 0 .3rem 0;">
    <n-grid cols="12">
      <n-gi span="2">
        <div class="nav-logo">
          <n-icon size="27">
            <svg xmlns="http://www.w3.org/2000/svg" xmlns:xlink="http://www.w3.org/1999/xlink" viewBox="0 0 24 24">
              <g fill="none">
                <path d="M5.25 2h5a.75.75 0 0 1 0 1.5h-5A1.75 1.75 0 0 0 3.5 5.25v3.126A4.264 4.264 0 0 0 2 9.51V5.25A3.25 3.25 0 0 1 5.25 2z" fill="currentColor" />
                <path d="M15.624 20.5A4.264 4.264 0 0 1 14.49 22h4.261A3.25 3.25 0 0 0 22 18.75v-5a.75.75 0 0 0-1.5 0v5a1.75 1.75 0 0 1-1.75 1.75h-3.126z" fill="currentColor" />
                <path d="M21.25 11a.75.75 0 0 0 .75-.75v-5A3.25 3.25 0 0 0 18.75 2h-5a.75.75 0 0 0 0 1.5h5c.966 0 1.75.784 1.75 1.75v5c0 .414.336.75.75.75z" fill="currentColor" />
                <path d="M5.25 9A3.25 3.25 0 0 0 2 12.25v6.5A3.25 3.25 0 0 0 5.25 22h6.5A3.25 3.25 0 0 0 15 18.75v-6.5A3.25 3.25 0 0 0 11.75 9h-6.5zM3.5 12.25c0-.966.784-1.75 1.75-1.75H6V13H3.5v-.75zM6 14.5V17H3.5v-2.5H6zM7.5 17v-2.5h2V17h-2zm3.5 0v-2.5h2.5V17H11zm-3.5 1.5h2v2h-2v-2zm3.5 0h2.5v.25a1.75 1.75 0 0 1-1.75 1.75H11v-2zm0-8h.75c.966 0 1.75.784 1.75 1.75V13H11v-2.5zM9.5 13h-2v-2.5h2V13zM6 18.5v2h-.75a1.75 1.75 0 0 1-1.75-1.75v-.25H6z" fill="currentColor" />
              </g>
            </svg>
          </n-icon>
          <n-text style="margin-left: 0.5rem">Taledynamic</n-text>
        </div>
      </n-gi>
      <n-gi span="6" offset="1">
        <div class="nav-menu">
          <n-menu :value="selectedMenuOption" :options="menuOptions" mode="horizontal" />
        </div>
      </n-gi>
      <n-gi span="2" offset="1">
        <div class="nav-extra">
          <n-button style="margin-right: 1rem">Light</n-button>
          <n-button text tag="a" href="https://github.com">Github</n-button>
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

const menuOptions = [
  {
    label: () =>
      h(
        RouterLink,
        {
          to: '/'
        },
        { default: () => 'Главная страница' }
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
        { default: () => 'Вход / Регистрация' }
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
  computed: {
    selectedMenuOption() {
      return this.$route.name;
    }
  },
}
</script>