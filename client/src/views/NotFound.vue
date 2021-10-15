<template>
    <n-result
    status="404"
    title="404 Not Found"
    description="You know life is always ridiculous." 
    size="huge"
    style="margin-top: 10rem"
    />
</template>

<script lang="ts">
import { defineComponent } from 'vue'

export default defineComponent({
    name: 'NotFound'
})
</script>
