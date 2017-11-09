INSERT INTO [dbo].[HelpDocuments]
           ([ControllerName]
           ,[ActionName]
           ,[HelpHtml]
           ,[UpdatedDate]
           ,[UpdatedBy]
           ,[CreatedDate]
           ,[CreatedBy])
     VALUES
           ('DailyDeposit',
           'Index',
		   '&lt;h3&gt;&lt;strong&gt;D&lt;sup&gt;2&lt;/sup&gt; &amp;ndash; DAILY DEPOSITS &lt;/strong&gt;&lt;/h3&gt;    &lt;p&gt;&lt;strong&gt;Cash deposits and change order returns should be entered into the D&lt;sup&gt;2&lt;/sup&gt; application at the beginning of each day for the previous day. All deposits for the week should be entered no later than _____ on Monday (?). &lt;/strong&gt;&lt;/p&gt;    &lt;p&gt;&amp;nbsp;&lt;/p&gt;    &lt;ol&gt;   &lt;li&gt;   &lt;p&gt;Open your Internet browser, navigate to &lt;a href=&quot;http://d2.millersalehouse.com&quot;&gt;http://d2.millersalehouse.com&lt;/a&gt;&lt;/p&gt;     &lt;ul&gt;    &lt;li&gt;    &lt;p&gt;&lt;em&gt;&lt;u&gt;Note:&lt;/u&gt; it is recommended to save this website as a Favorite for easy access in the future.&lt;/em&gt;&lt;/p&gt;      &lt;p&gt;&lt;em&gt;Will the stores already have login credentials or will they be automatically taken to their store?&lt;/em&gt;&lt;/p&gt;    &lt;/li&gt;   &lt;/ul&gt;   &lt;/li&gt;   &lt;li&gt;Take note of the restaurant number and period (week) ending date indicated in the heading of the Daily Deposits screen. If either is incorrect, contact the IT Service Desk at 407-581-3366 or &lt;a href=&quot;mailto:ITService@millersalehouse.com&quot;&gt;ITService@millersalehouse.com&lt;/a&gt;&lt;br /&gt;   &amp;nbsp;&lt;/li&gt;   &lt;li&gt;Input Cash Deposits and Change Order Returns (if applicable) for the appropriate day.   &lt;ul&gt;    &lt;li&gt;    &lt;p&gt;&lt;em&gt;&lt;u&gt;Note:&lt;/u&gt; Cash Deposits include all cash deposits other than change order returns.&lt;/em&gt;&lt;/p&gt;    &lt;/li&gt;   &lt;/ul&gt;   &lt;/li&gt;   &lt;li&gt;Click on &lt;em&gt;&amp;lsquo;Save Changes.&amp;rsquo;&lt;/em&gt;&lt;br /&gt;   &amp;nbsp;&lt;/li&gt;   &lt;li&gt;Repeat steps 1) through 4) at the beginning of each day for the previous day&amp;rsquo;s deposits and change order returns.&lt;br /&gt;   &amp;nbsp;&lt;/li&gt;   &lt;li&gt;Deposits for the week will be automatically transmitted into Microsoft Dynamics AX (general ledger system) every Monday (?) (when?). Therefore, &lt;strong&gt;it is imperative that all deposits for the week have been entered and saved no later than _______.&lt;/strong&gt;&lt;br /&gt;   &amp;nbsp;&lt;/li&gt;   &lt;li&gt;Contact the IT Service Desk for any issues at the phone number or email address noted in 2) above.&lt;br /&gt;   &amp;nbsp;&lt;/li&gt;  &lt;/ol&gt;    &lt;p&gt;&amp;nbsp;&lt;/p&gt;',
           GetDate(),
		   'Seed Method',
		   GetDate(),
		   'SeedMethod'),

		   ('SalesForecast',
           'Index',
		   '&lt;h3&gt;&lt;strong&gt;D&lt;sup&gt;2&lt;/sup&gt; &amp;ndash; DAILY DEPOSITS &lt;/strong&gt;&lt;/h3&gt;    &lt;p&gt;&lt;strong&gt;Cash deposits and change order returns should be entered into the D&lt;sup&gt;2&lt;/sup&gt; application at the beginning of each day for the previous day. All deposits for the week should be entered no later than _____ on Monday (?). &lt;/strong&gt;&lt;/p&gt;    &lt;p&gt;&amp;nbsp;&lt;/p&gt;    &lt;ol&gt;   &lt;li&gt;   &lt;p&gt;Open your Internet browser, navigate to &lt;a href=&quot;http://d2.millersalehouse.com&quot;&gt;http://d2.millersalehouse.com&lt;/a&gt;&lt;/p&gt;     &lt;ul&gt;    &lt;li&gt;    &lt;p&gt;&lt;em&gt;&lt;u&gt;Note:&lt;/u&gt; it is recommended to save this website as a Favorite for easy access in the future.&lt;/em&gt;&lt;/p&gt;      &lt;p&gt;&lt;em&gt;Will the stores already have login credentials or will they be automatically taken to their store?&lt;/em&gt;&lt;/p&gt;    &lt;/li&gt;   &lt;/ul&gt;   &lt;/li&gt;   &lt;li&gt;Take note of the restaurant number and period (week) ending date indicated in the heading of the Daily Deposits screen. If either is incorrect, contact the IT Service Desk at 407-581-3366 or &lt;a href=&quot;mailto:ITService@millersalehouse.com&quot;&gt;ITService@millersalehouse.com&lt;/a&gt;&lt;br /&gt;   &amp;nbsp;&lt;/li&gt;   &lt;li&gt;Input Cash Deposits and Change Order Returns (if applicable) for the appropriate day.   &lt;ul&gt;    &lt;li&gt;    &lt;p&gt;&lt;em&gt;&lt;u&gt;Note:&lt;/u&gt; Cash Deposits include all cash deposits other than change order returns.&lt;/em&gt;&lt;/p&gt;    &lt;/li&gt;   &lt;/ul&gt;   &lt;/li&gt;   &lt;li&gt;Click on &lt;em&gt;&amp;lsquo;Save Changes.&amp;rsquo;&lt;/em&gt;&lt;br /&gt;   &amp;nbsp;&lt;/li&gt;   &lt;li&gt;Repeat steps 1) through 4) at the beginning of each day for the previous day&amp;rsquo;s deposits and change order returns.&lt;br /&gt;   &amp;nbsp;&lt;/li&gt;   &lt;li&gt;Deposits for the week will be automatically transmitted into Microsoft Dynamics AX (general ledger system) every Monday (?) (when?). Therefore, &lt;strong&gt;it is imperative that all deposits for the week have been entered and saved no later than _______.&lt;/strong&gt;&lt;br /&gt;   &amp;nbsp;&lt;/li&gt;   &lt;li&gt;Contact the IT Service Desk for any issues at the phone number or email address noted in 2) above.&lt;br /&gt;   &amp;nbsp;&lt;/li&gt;  &lt;/ol&gt;    &lt;p&gt;&amp;nbsp;&lt;/p&gt;',
           GetDate(),
		   'Seed Method',
		   GetDate(),
		   'SeedMethod')
GO


