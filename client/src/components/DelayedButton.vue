<template>
	<n-button-group>
		<n-button
			:attr-type="attrType"
			:ghost="ghost"
			@click="emit('click', $event)"
			:loading="loading"
			:disabled="disabled || loading || (disablingDuration != 0)"
			:type="type"
			:style="contentStyle"
			>
			<slot></slot>
		</n-button>
		<n-button v-if="disablingDuration" disabled :type="type" ghost>{{ disablingDuration }}</n-button>
	</n-button-group>
</template>

<script setup lang="ts">
import { ref, PropType } from 'vue'

/* global defineProps defineEmits defineExpose */
const emit = defineEmits(['click'])
const props = defineProps({
	attrType: {
		type: String as PropType<'button' | 'submit' | 'reset'>,
		default: 'button'
	},
	loading: {
		type: Boolean,
		default: false
	},
	type: {
		type: String as PropType<'default' | 'primary' | 'success' | 'info' | 'warning' |'error'>,
		default: 'default'
	},
	duration: {
		type: Number,
		default: 15
	},
	ghost: {
		type: Boolean,
		default: false
	},
	disabled: {
		type: Boolean,
		default: false
	},
	contentStyle: {
		type: [Object, String]
	}
})

const disablingDuration = ref<number>(0);

let disablingTimer: number;

function holdDisabled(): Promise<void> {
	return new Promise<void>(resolve => {
		disablingDuration.value = props.duration;
		disablingTimer = setInterval(() => {
			disablingDuration.value--;
			if (disablingDuration.value === 0) {
				clearInterval(disablingTimer);
				resolve();
			}
		}, 1000);
	})
}

function cancelHolding(): void {
	clearInterval(disablingTimer);
	disablingDuration.value = 0;
}

defineExpose({
	holdDisabled,
	cancelHolding
})
</script>
