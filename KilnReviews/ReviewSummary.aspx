<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="ReviewSummary.aspx.cs" Inherits="KilnReviews.ReviewSummary" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Open Code Reviews</title>
	<link rel="stylesheet" type="text/css" href="styles.css"/>
    <link href="bootstrap/css/bootstrap.css" rel="stylesheet" />
</head>
<body>

    <div class="container">
	<h3>Code Reviews for <%= UserName %></h3>
	<div id="reviewsTodo" class="fetching"></div>
	<div id="reviewsRejected" class="fetching"></div>
	<div id="reviewsWaiting" class="fetching"></div>
	<div id="casesReady" class="fetching"></div>
	
    <script type="text/x-jquery-tmpl" id="reviewsTableTemplate">
		<table class="table table-striped table-hover">
			<thead>
				<tr>
					<th>Review</th>
					<th>Days&nbsp;old</th>
					<th>Who</th>
					<th>UI?</th>
				</tr>
			</thead>
			<tbody>
				{{each items}}
					<tr>
						<td><span class="reviewTitle"><a href="<%= ConfigurationManager.AppSettings["kilnUrlBase"] %>Review/${sReview}">${sReview}: ${sTitle}</a></span></td>
						<td><span class="reviewAge{{if DaysOld > 14}} ancient{{/if}}">${DaysOld}</span></td>
						<td>
							{{if (params.reviewing) }}
								{{each Authors}} 
									<img src="${$value}"/>
								{{/each}}
							{{else}}
							{{each Reviewers}} 
									<img src="${$value}"/>
								{{/each}}

							{{/if}}
						</td>
						<td>
							{{if ContainsXamlFiles}}
							<img src="images/contains-xaml.png" alt="UI"/>
							{{else}}
							<img src="images/no-xaml.png" alt=""/>
							{{/if}}
						</td>
					</tr>
				{{/each}}
			</tbody>
		</table>
    </script>

	<script type="text/x-jquery-tmpl" id="casesTableTemplate">
		<table class="table table-striped table-hover">
			<thead>
				<tr>
					<th>Case</th>
				</tr>
			</thead>
			<tbody>
				{{each items}}
					<tr>
						<td><span class="reviewTitle"><a href="<%= ConfigurationManager.AppSettings["fogBugzUrlBase"] %>default.asp?${ixBug}">${ixBug}: ${sTitle}</a></span></td>
					</tr>
				{{/each}}
			</tbody>
		</table>
    </script>

	<script type="text/x-jquery-tmpl" id="mainTemplate">
		<h3>${title}</h3>

	    {{if items.length > 0}}
            <div class="row">
                <div class="span12">
					{{tmpl({items: items, params: params}) '#'+itemTableTemplate}}
                </div>
            </div>
		{{else}}
			<span class="noItems">None</span>
		{{/if}}
	</script>

	<script type="text/javascript" src="Scripts/jquery-1.8.3.min.js"></script>
	<script type="text/javascript" src="Scripts/jQuery.tmpl.min.js"></script>
	<script type="text/javascript" src="Scripts/spine/spine.js"></script>
	<script type="text/javascript" src="Scripts/spine/ajax.js"></script>

	<script type="text/javascript" src="main.js"></script>
    </div>
</body>
</html>
