<template>
  <div class="container">
    <n-grid :cols="12" :x-gap="20">
      <n-gi :span="3">
        <workspaces-section />
      </n-gi>
      <n-gi :span="9">
        <transition name="fade" mode="out-in">
          <tables-section v-if="store.getters['workspaces/currentWorkspaceId']" />
        </transition>
      </n-gi>
    </n-grid>
  </div>
</template>

<script setup lang="ts">
import { useStore } from '@/store';
import WorkspacesSection from '@/components/workspaces/WorkspacesSection.vue';
import TablesSection from '@/components/tables/TablesSection.vue';

const store = useStore();

</script>

<style lang="scss" scoped>
</style>
