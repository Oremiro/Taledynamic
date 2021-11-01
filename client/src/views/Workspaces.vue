<template>
	<div class="container">
		<n-list>
			<n-list-item>
				My First Workspace
			</n-list-item>
		</n-list>
	</div>
</template>

<style lang="scss" scoped>
.container {
  width: 100%;
  height: 100%;
  display: flex;
  justify-content: center;
  align-items: center;
}
</style>

<script lang="ts" setup>
import { NList, NListItem } from 'naive-ui';
</script>