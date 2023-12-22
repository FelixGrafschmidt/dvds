<template>
	<main class="flex flex-col">
		<div class="mb-2">
			<button @click="$router.push('/')" class="bg-teal-7 py-1 rounded px-2 hover:bg-teal-6">Back</button>
		</div>
		<div v-if="movie" class="flex flex-col gap-2">
			<h2 class="text-xl">{{ movie.title }}</h2>
			<span>Description:</span>
			<p class="max-w-96">{{ movie.description }}</p>
			<span>Starring:</span>
			<div class="flex flex-col gap-1">
				<span v-for="(actor, i) in movie.actors.split(', ')" :key="i">
					{{ actor }}
				</span>
			</div>
		</div>
		<div v-else>No movie with this ID</div>
	</main>
</template>

<script setup lang="ts">
	import { ref, type Ref } from "vue";
	import { useRoute } from "vue-router";

	const movie: Ref<null | DetailMovie> = ref(null);

	const id = ref(parseInt(useRoute().params.id.toString()));

	try {
		const response = await fetch(import.meta.env.VITE_API_HOST + "/movie-details/" + id.value);
		movie.value = await response.json();
	} catch (error) {
		console.error(error);
	}
</script>
