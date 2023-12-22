import { createRouter, createWebHistory } from "vue-router";
import ListView from "../views/ListView.vue";
import DetailView from "../views/DetailView.vue";

const router = createRouter({
	history: createWebHistory(import.meta.env.BASE_URL),
	routes: [
		{
			path: "/",
			name: "list",
			component: ListView,
		},
		{
			path: "/:id",
			name: "details",
			component: DetailView,
		},
	],
});

export default router;
