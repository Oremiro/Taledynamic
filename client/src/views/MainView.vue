<template>
  <div class="container">
    <n-grid v-if="store.getters['workspaces/currentWorkspaceId']" :cols="12" :x-gap="20">
      <n-gi :span="3">
        <workspaces-section />
      </n-gi>
      <n-gi :span="9">
        <tables-section />
      </n-gi>
    </n-grid>
    <workspaces-section v-else />
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
