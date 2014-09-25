KilnReviewDashboard
===================

Provides an overview of the status of your code reviews in kiln.

Conveniently display all of your active [Kiln](https://www.fogcreek.com/kiln/) [Code Reviews](https://www.fogcreek.com/kiln/features/code-reviews.html) in a single web page.

* Reviews you need to do
* Rejected reviews you need to fix
* In progress reviews of your code

To use it, set `kilnUrlBase` in the web.config to your kiln base url, e.g., https://example.kilnhg.com/, and log in with your FogBugz details.

### FogBugz Integration

You can also integrate with FogBugz, which will add a section showing you:

* All your FogBugz cases with a specific status, where all reviews have been approved.

Thus, if you your process is that code reviews must be completed before testing, this will show you which of your cases are ready to be moved to testing.

To use the FogBugz integration:

* Set 'fogBugzUrlBase` in the web.config to your FogBugz base url, e.g. https://example.fogbugz.com/.
* Set 'fogBugzAwaitingReviewStatus' in the web.config to the name of the status you use for cases awaiting review.
