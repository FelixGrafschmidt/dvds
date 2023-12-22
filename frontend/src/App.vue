<template>
	<div class="bg-gray-6 text-gray-2 min-h-screen">
		<header class="px-16 py-2 border border-b border-black">
			<h1 class="text-2xl">DVD-Rental</h1>
		</header>
		<Suspense>
			<RouterView class="py-4 px-16" />
		</Suspense>
	</div>
</template>

<script setup lang="ts">
	import { RouterView } from "vue-router";
</script>
