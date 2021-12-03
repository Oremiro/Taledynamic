<template>
  <div class="container">
    <n-grid :cols="12" :x-gap="20">
      <n-gi :span="3">
        <workspaces-section />
      </n-gi>
      <n-gi :span="9">
        <tables-section />
      </n-gi>
    </n-grid>
  </div>
</template>

<script setup lang="ts">
import WorkspacesSection from '@/components/workspaces/WorkspacesSection.vue';
import TablesSection from '@/components/tables/TablesSection.vue';
</script>

<style lang="scss" scoped>
</style>
