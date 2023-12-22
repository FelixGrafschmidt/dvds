<template>
	<main class="flex flex-col">
		<div class="mb-2 flex flex-row">
			<input
				@keydown="handleKeydown"
				v-model="filter"
				type="text"
				class="bg-gray-3 text-gray-8 rounded-l px-2 py-1"
				placeholder="Filter title"
			/>
			<button @click="load" class="bg-teal-7 py-1 rounded-r px-2 hover:bg-teal-6">Filter</button>
		</div>
		<div class="flex flex-row gap-2 text-xl max-w-7xl mb-2 px-2">
			<span class="w-1/3 flex justify-start">Title</span>
			<span class="w-1/3 justify-center flex">Language</span>
			<span class="w-1/3 flex justify-end">Release Year</span>
		</div>
		<RouterLink
			v-for="(movie, i) in movies"
			class="flex flex-row max-w-7xl even:hover:bg-teal-6 even:bg-gray-5 p-2 hover:bg-teal-6"
			:key="i"
			:to="movie.id.toString()"
		>
			<span class="w-1/3 flex justify-start">{{ movie.title }}</span>
			<span class="w-1/3 justify-center flex">{{ movie.language }}</span>
			<span class="w-1/3 flex justify-end">{{ movie.releaseYear }}</span>
		</RouterLink>
	</main>
</template>

<script setup lang="ts">
	import { ref, type Ref } from "vue";

	const filter = ref("");

	const movies: Ref<ListMovie[]> = ref([]);

	async function load() {
		try {
			const response = await fetch(import.meta.env.VITE_API_HOST + "/movies?filter=" + filter.value);
			movies.value = await response.json();
		} catch (error) {
			console.error(error);
		}
	}

	async function handleKeydown(event: KeyboardEvent) {
		if (event.key === "Enter") {
			await load();
		}
	}

	await load();
</script>
