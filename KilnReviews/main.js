$(function () {
	var ReviewTodo = Spine.Model.sub();
	ReviewTodo.configure("Review", "sReview", "sStatus", "sTitle", "reviewers");
	ReviewTodo.extend(Spine.Model.Ajax);
	ReviewTodo.extend({ url: "api/Reviews/Todo" });

	var ReviewRejected = Spine.Model.sub();
	ReviewRejected.configure("Review", "sReview", "sStatus", "sTitle", "reviewers");
	ReviewRejected.extend(Spine.Model.Ajax);
	ReviewRejected.extend({ url: "api/Reviews/Rejected" });

	var ReviewWaiting = Spine.Model.sub();
	ReviewWaiting.configure("Review", "sReview", "sStatus", "sTitle", "reviewers");
	ReviewWaiting.extend(Spine.Model.Ajax);
	ReviewWaiting.extend({ url: "api/Reviews/Waiting" });

	var CaseReady = Spine.Model.sub();
	CaseReady.configure("Case", "sStatus", "sTitle");
	CaseReady.extend(Spine.Model.Ajax);
	CaseReady.extend({ url: "api/Cases/Ready" });

	var ReviewsController = Spine.Controller.sub({
		elements: {
			"#reviewsTodo": "reviewsTodo",
			"#reviewsRejected": "reviewsRejected",
			"#reviewsWaiting": "reviewsWaiting",
			"#casesReady": "casesReady",
		},

		init: function () {
			ReviewTodo.bind("refresh", this.proxy(this.addReviewsTodo));
			ReviewRejected.bind("refresh", this.proxy(this.addReviewsRejected));
			ReviewWaiting.bind("refresh", this.proxy(this.addReviewsWaiting));
			CaseReady.bind("refresh", this.proxy(this.addCasesReady));

			ReviewTodo.fetch();
			ReviewRejected.fetch();
			ReviewWaiting.fetch();
			CaseReady.fetch();
		},

		template: function (title, items, itemTableTemplate, params) {
			return $('#mainTemplate').tmpl({
				title: title,
				itemTableTemplate: itemTableTemplate,
				items: items,
				params: params
			});
		},
		
		refreshList: function (element, title, items, reviewing, templateId) {
			element.html(this.template(title, items, reviewing, templateId));
			element.removeClass("fetching");
		},
		
		addReviewsTodo: function () {
			this.refreshList($(this.reviewsTodo), "Reviews to do:", ReviewTodo.all(), 'reviewsTableTemplate', { reviewing: true });
		},

		addReviewsRejected: function () {
			this.refreshList($(this.reviewsRejected), "Reviews to fix:", ReviewRejected.all(), 'reviewsTableTemplate', { reviewing: false });
		},

		addReviewsWaiting: function () {
			this.refreshList($(this.reviewsWaiting), "Your code under review:", ReviewWaiting.all(), 'reviewsTableTemplate', { reviewing: false });
		},

		addCasesReady: function() {
			this.refreshList($(this.casesReady), "Your cases passed review:", CaseReady.all(), 'casesTableTemplate', {});
		}
	});
	
	new ReviewsController({ el: $("body") });
});