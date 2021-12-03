<template>
  <div class="container">
    <n-grid :cols="12" :x-gap="20">
      <n-gi :span="3">
        <n-card size="small">
          <workspaces-section />
        </n-card>
      </n-gi>
      <n-gi :span="9">
        <n-card>
          
        </n-card>
      </n-gi>
    </n-grid>
  </div>
</template>

<script setup lang="ts">
import WorkspacesSection from '@/components/workspaces/WorkspacesSection.vue';
</script>

<style lang="scss" scoped>
</style>
