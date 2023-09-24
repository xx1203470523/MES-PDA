export type HomeItemType = {
	icon: string;
	title: string;
	url?: string;
	describe?: string;
}

export type HomeCollapseType = {
	title: string;
	items: Array<HomeItemType>;
}

export type HomeType = {
	windowInfo ?: UniNamespace.GetWindowInfoResult
	collapse: {
		items: Array<HomeCollapseType>;
		open: Array<string>;
	}
}