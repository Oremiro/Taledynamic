<template>
  <div class="card-header">
    <div class="user-img-background">
      <img class="user-img" src="https://picsum.photos/500" alt="" />
    </div>
    <div class="user-info-background">
      <n-text type="primary" style="font-size: 1.25rem">
        {{ store.getters["user/email"] }}
      </n-text>
    </div>
  </div>
</template>

<script setup lang="ts">
import { useStore } from "@/store";

const store = useStore();
</script>

<style lang="scss" scoped>
.card-header {
  display: flex;
  justify-content: space-between;
  padding-bottom: var(--padding-bottom);
}
.user-img {
  max-width: 10rem;
  max-height: 10rem;
  border-radius: var(--border-radius);
}
.user-img-background {
  display: flex;
  justify-content: center;
}
.user-info-background {
  display: flex;
  width: 100%;
  flex-direction: column;
  padding-left: 2rem;
}
</style>
