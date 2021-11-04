<template>
	<n-button 
		@mouseenter="mouseEnterHandler" 
		@mouseleave="mouseLeaveHandler" 
		:type="isHovered ? type : 'default'" 
		:ghost="ghost">
		<slot></slot>
		<template #icon v-if="$slots.icon">
			<slot name="icon"></slot>
		</template>
	</n-button>
</template>
<script setup lang="ts">
import { ref, PropType } from 'vue';


/* global defineProps */
defineProps({
	type: {
		required: true,
		type: String as PropType<'success' | 'info' | 'warning' | 'error'>
	},
	ghost: {
		type: Boolean,
		default: false
	}
})

const isHovered = ref<boolean>(false)

function mouseEnterHandler() {
	isHovered.value = true;
}

function mouseLeaveHandler() {
	isHovered.value = false;
}
</script>