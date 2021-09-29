<template>
  <div id="nav">
    <router-link to="/">Главная страница</router-link>
    <div>
      <router-link to="/signin">Вход</router-link>
      <router-link to="/signup">Регистрация</router-link>
    </div>
  </div>
  <router-view></router-view>
</template>

<style lang="scss">

body {
  margin: 0;
}
#app {
  font-family: Avenir, Helvetica, Arial, sans-serif;
  -webkit-font-smoothing: antialiased;
  -moz-osx-font-smoothing: grayscale;
  text-align: center;
  color: #2c3e50;
  min-height: 100vh;
}

#nav {
  display: flex;
  justify-content: space-between;
  padding: 1rem;
  border-bottom: rgba(44, 62, 80, 0.2) solid thin;
  a {
    font-weight: bold;
    color: #2c3e50;
    margin: 0 1rem;
    text-decoration-line: none;

    &.router-link-exact-active {
      color: #42b983;
    }
  }
}
</style>
