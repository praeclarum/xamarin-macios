﻿//
// Unit tests for CPMessageListItem
//
// Authors:
//	Whitney Schmidt <whschm@microsoft.com>
//
// Copyright (c) Microsoft Corporation.
//

#if __IOS__
using System;
using NUnit.Framework;

using CarPlay;
using Foundation;

namespace MonoTouchFixtures.CarPlay {

	[TestFixture]
	[Preserve (AllMembers = true)]
	public class CPMessageListItemTest {

		[SetUp]
		public void Setup () => TestRuntime.AssertXcodeVersion (12, 0);

		[Test]
		public void InitUsingConversationIdentifier ()
		{
			var leadingItemConfig = new CPMessageListItemLeadingConfiguration (new CPMessageLeadingItem (), null, false);
			var trailingItemConfig = new CPMessageListItemTrailingConfiguration (new CPMessageTrailingItem (), null);
			CPMessageListItem listItem = new CPMessageListItem ("convoId", "text", leadingItemConfig, trailingItemConfig, "detailText", "trailingText", CPMessageListItemType.Identifier);

			Assert.NotNull (listItem, "CPMessageListItem not be null.");
			Assert.AreEqual (listItem.Text, "text");
			Assert.AreEqual (listItem.ConversationIdentifier, "convoId");
			Assert.AreSame (listItem.LeadingConfiguration, leadingItemConfig);
			Assert.AreSame (listItem.TrailingConfiguration, trailingItemConfig);
			Assert.AreEqual (listItem.DetailText, "detailText");
			Assert.AreEqual (listItem.TrailingText, "trailingText");
		}

		[Test]
		public void InitUsingFullName ()
		{
			var leadingItemConfig = new CPMessageListItemLeadingConfiguration (new CPMessageLeadingItem (), null, false);
			var trailingItemConfig = new CPMessageListItemTrailingConfiguration (new CPMessageTrailingItem (), null);
			CPMessageListItem listItem = new CPMessageListItem ("fullName", "phoneOrEmail", leadingItemConfig, trailingItemConfig, "detailText", "trailingText", CPMessageListItemType.FullName);

			Assert.NotNull (listItem, "CPMessageListItem not be null.");
			Assert.AreEqual (listItem.Text, "fullName");
			Assert.AreEqual (listItem.PhoneOrEmailAddress, "phoneOrEmail");
			Assert.AreSame (listItem.LeadingConfiguration, leadingItemConfig);
			Assert.AreSame (listItem.TrailingConfiguration, trailingItemConfig);
			Assert.AreEqual (listItem.DetailText, "detailText");
			Assert.AreEqual (listItem.TrailingText, "trailingText");
		}

	}
}
#endif // __IOS__
