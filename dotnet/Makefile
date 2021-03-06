TOP = ..

include $(TOP)/Make.config
include $(TOP)/mk/rules.mk

define DefineTargets
$(1)_NUGET_TARGETS = \
	$(DOTNET_DESTDIR)/Microsoft.$(1).Sdk/Sdk/Sdk.targets \
	$(DOTNET_DESTDIR)/Microsoft.$(1).Sdk/Sdk/Sdk.props \
	$(DOTNET_DESTDIR)/Microsoft.$(1).Sdk/targets/Microsoft.$(1).Sdk.SupportedTargetPlatforms.props \
	$(DOTNET_DESTDIR)/Microsoft.$(1).Sdk/targets/Microsoft.$(1).Sdk.DefaultItems.props \
	$(DOTNET_DESTDIR)/Microsoft.$(1).Sdk/targets/Microsoft.$(1).Sdk.props \
	$(DOTNET_DESTDIR)/Microsoft.$(1).Sdk/targets/Microsoft.$(1).Sdk.targets \
	$(DOTNET_DESTDIR)/Microsoft.$(1).Sdk/targets/Microsoft.$(1).TargetFrameworkInference.targets \
	$(DOTNET_DESTDIR)/Microsoft.$(1).Sdk/targets/Xamarin.Shared.Sdk.DefaultItems.props \
	$(DOTNET_DESTDIR)/Microsoft.$(1).Sdk/targets/Xamarin.Shared.Sdk.DefaultItems.targets \
	$(DOTNET_DESTDIR)/Microsoft.$(1).Sdk/targets/Xamarin.Shared.Sdk.props \
	$(DOTNET_DESTDIR)/Microsoft.$(1).Sdk/targets/Xamarin.Shared.Sdk.TargetFrameworkInference.targets \
	$(DOTNET_DESTDIR)/Microsoft.$(1).Sdk/targets/Xamarin.Shared.Sdk.targets \
	$(DOTNET_DESTDIR)/Microsoft.$(1).Sdk/targets/Xamarin.Shared.Sdk.Versions.props \
	$(DOTNET_DESTDIR)/Microsoft.$(1).Sdk/targets/Xamarin.Shared.Sdk.Versions.template.props
endef
$(foreach platform,$(DOTNET_PLATFORMS),$(eval $(call DefineTargets,$(platform))))

DIRECTORIES += \
	$(DOTNET_NUPKG_DIR) \
	$(DOTNET_FEED_DIR) \
	$(foreach platform,$(DOTNET_PLATFORMS),$(DOTNET_DESTDIR)/Microsoft.$(platform).Sdk/Sdk) \
	$(foreach platform,$(DOTNET_PLATFORMS),$(DOTNET_DESTDIR)/Microsoft.$(platform).Sdk/targets) \

$(DIRECTORIES):
	$(Q) mkdir -p $@

CURRENT_HASH_LONG:=$(shell git log -1 --pretty=%H)

$(DOTNET_DESTDIR)/Microsoft.%: Microsoft.% | $(foreach platform,$(DOTNET_PLATFORMS),$(DOTNET_DESTDIR)/Microsoft.$(platform).Sdk/Sdk $(DOTNET_DESTDIR)/Microsoft.$(platform).Sdk/targets)
	$(Q) $(CP) $< $@

$(DOTNET_DESTDIR)/Microsoft.iOS.Sdk/targets/%: targets/% | $(DOTNET_DESTDIR)/Microsoft.iOS.Sdk/targets
	$(Q) $(CP) $< $@

$(DOTNET_DESTDIR)/Microsoft.tvOS.Sdk/targets/%: targets/% | $(DOTNET_DESTDIR)/Microsoft.tvOS.Sdk/targets
	$(Q) $(CP) $< $@

$(DOTNET_DESTDIR)/Microsoft.watchOS.Sdk/targets/%: targets/% | $(DOTNET_DESTDIR)/Microsoft.watchOS.Sdk/targets
	$(Q) $(CP) $< $@

$(DOTNET_DESTDIR)/Microsoft.macOS.Sdk/targets/%: targets/% | $(DOTNET_DESTDIR)/Microsoft.macOS.Sdk/targets
	$(Q) $(CP) $< $@

targets/%.props: targets/%.template.props Makefile $(TOP)/Make.config.inc $(TOP)/.git/HEAD $(TOP)/.git/index
	$(Q_GEN) sed \
		-e "s/@IOS_NUGET_VERSION_NO_METADATA@/$(IOS_NUGET_VERSION_NO_METADATA)/g" \
		-e "s/@TVOS_NUGET_VERSION_NO_METADATA@/$(TVOS_NUGET_VERSION_NO_METADATA)/g" \
		-e "s/@WATCHOS_NUGET_VERSION_NO_METADATA@/$(WATCHOS_NUGET_VERSION_NO_METADATA)/g" \
		-e "s/@MACOS_NUGET_VERSION_NO_METADATA@/$(MACOS_NUGET_VERSION_NO_METADATA)/g" \
		-e "s/@IOS_NUGET_VERSION_FULL@/$(IOS_NUGET_VERSION_FULL)/g" \
		-e "s/@TVOS_NUGET_VERSION_FULL@/$(TVOS_NUGET_VERSION_FULL)/g" \
		-e "s/@WATCHOS_NUGET_VERSION_FULL@/$(WATCHOS_NUGET_VERSION_FULL)/g" \
		-e "s/@MACOS_NUGET_VERSION_FULL@/$(MACOS_NUGET_VERSION_FULL)/g" \
		-e "s/@CURRENT_BRANCH@/$(CURRENT_BRANCH_SED_ESCAPED)/g" \
		-e "s/@CURRENT_HASH_LONG@/$(CURRENT_HASH_LONG)/g" \
	$< > $@

define SupportedTargetPlatforms
Microsoft.$(1).Sdk/targets/Microsoft.$(1).Sdk.SupportedTargetPlatforms.props: $(TOP)/Versions-ios.plist.in $(TOP)/Versions-mac.plist.in Makefile ./generate-target-platforms.csharp Makefile
	$(Q) rm -f $$@.tmp
	$(Q) ./generate-target-platforms.csharp $(1) $$@.tmp
	$(Q) mv $$@.tmp $$@
endef
$(foreach platform,$(DOTNET_PLATFORMS),$(eval $(call SupportedTargetPlatforms,$(platform))))

TEMPLATED_FILES = \
	targets/Xamarin.Shared.Sdk.Versions.props \
	$(foreach platform,$(DOTNET_PLATFORMS),Microsoft.$(platform).Sdk/targets/Microsoft.$(platform).Sdk.SupportedTargetPlatforms.props) \

nupkgs/$(IOS_NUGET).%.nupkg: CURRENT_VERSION_NO_METADATA=$(IOS_NUGET_VERSION_NO_METADATA)
nupkgs/$(TVOS_NUGET).%.nupkg: CURRENT_VERSION_NO_METADATA=$(TVOS_NUGET_VERSION_NO_METADATA)
nupkgs/$(WATCHOS_NUGET).%.nupkg: CURRENT_VERSION_NO_METADATA=$(WATCHOS_NUGET_VERSION_NO_METADATA)
nupkgs/$(MACOS_NUGET).%.nupkg: CURRENT_VERSION_NO_METADATA=$(MACOS_NUGET_VERSION_NO_METADATA)

nupkgs/$(IOS_NUGET).%.nupkg: CURRENT_VERSION_FULL=$(IOS_NUGET_VERSION_FULL)
nupkgs/$(TVOS_NUGET).%.nupkg: CURRENT_VERSION_FULL=$(TVOS_NUGET_VERSION_FULL)
nupkgs/$(WATCHOS_NUGET).%.nupkg: CURRENT_VERSION_FULL=$(WATCHOS_NUGET_VERSION_FULL)
nupkgs/$(MACOS_NUGET).%.nupkg: CURRENT_VERSION_FULL=$(MACOS_NUGET_VERSION_FULL)

# Create the nuget in a temporary directory (nupkgs/)
define CreateNuGetTemplate
nupkgs/$(1).$(2)+$(NUGET_BUILD_METADATA).nupkg: $(TEMPLATED_FILES) $(3) package/$(1)/package.csproj $(wildcard package/*.csproj) $(wildcard $(DOTNET_DESTDIR)/$(1)/* $(DOTNET_DESTDIR)/$(1)/*/* $(DOTNET_DESTDIR)/$(1)/*/*/* $(DOTNET_DESTDIR)/$(1)/*/*/*/*) global.json
	@# Delete any versions of the nuget we're building
	$$(Q) rm -f nupkgs/$(1).*.nupkg
	$$(Q_PACK) $(DOTNET5) pack package/$(1)/package.csproj --output "$$(dir $$@)" $(DOTNET_PACK_VERBOSITY)
	@# Nuget pack doesn't add the metadata to the filename, but we want that, so rename nuget to contain the full name
	$$(Q) mv "nupkgs/$(1).$(2).nupkg" "$$@"
	@# Clean the local feed
	$$(Q_NUGET_DEL) if test -d $(DOTNET_FEED_DIR)/$(shell echo $(1) | tr A-Z a-z)/$(2); then nuget delete $(1) $(2) -source $(abspath $(DOTNET_FEED_DIR)) -NonInteractive $(NUGET_VERBOSITY); fi
	@# Add the nupkg to our local feed
	$$(Q_NUGET_ADD) nuget add "$$@" -source $(DOTNET_FEED_DIR) -NonInteractive $(NUGET_VERBOSITY)
endef

# Create variables prefixed with the correctly cased platform name from the upper-cased platform name. This makes the next section somewhat simpler.
$(foreach platform,$(DOTNET_PLATFORMS),$(eval $(platform)_NUGET_VERSION_NO_METADATA:=$($(shell echo $(platform) | tr a-z A-Z)_NUGET_VERSION_NO_METADATA)))

# Create the NuGet packaging targets. It's amazing what make allows you to do...
$(foreach platform,$(DOTNET_PLATFORMS),$(eval $(call CreateNuGetTemplate,Microsoft.$(platform).Sdk,$($(platform)_NUGET_VERSION_NO_METADATA),$($(platform)_NUGET_TARGETS))))
$(foreach platform,$(DOTNET_PLATFORMS),$(eval $(call CreateNuGetTemplate,Microsoft.$(platform).Ref,$($(platform)_NUGET_VERSION_NO_METADATA))))
$(foreach platform,$(DOTNET_PLATFORMS),$(foreach rid,$(DOTNET_$(platform)_RUNTIME_IDENTIFIERS),$(eval $(call CreateNuGetTemplate,Microsoft.$(platform).Runtime.$(rid),$($(platform)_NUGET_VERSION_NO_METADATA)))))

# Copy the nuget from the temporary directory into the final directory
$(DOTNET_NUPKG_DIR)/%.nupkg: nupkgs/%.nupkg | $(DOTNET_NUPKG_DIR)
	$(Q) $(CP) $< $@

RUNTIME_PACKS_IOS = $(foreach rid,$(DOTNET_IOS_RUNTIME_IDENTIFIERS),$(DOTNET_NUPKG_DIR)/$(IOS_NUGET).Runtime.$(rid).$(IOS_NUGET_VERSION_FULL).nupkg)
RUNTIME_PACKS_TVOS = $(foreach rid,$(DOTNET_TVOS_RUNTIME_IDENTIFIERS),$(DOTNET_NUPKG_DIR)/$(TVOS_NUGET).Runtime.$(rid).$(TVOS_NUGET_VERSION_FULL).nupkg)
RUNTIME_PACKS_WATCHOS = $(foreach rid,$(DOTNET_WATCHOS_RUNTIME_IDENTIFIERS),$(DOTNET_NUPKG_DIR)/$(WATCHOS_NUGET).Runtime.$(rid).$(WATCHOS_NUGET_VERSION_FULL).nupkg)
RUNTIME_PACKS_MACOS = $(foreach rid,$(DOTNET_MACOS_RUNTIME_IDENTIFIERS),$(DOTNET_NUPKG_DIR)/$(MACOS_NUGET).Runtime.$(rid).$(MACOS_NUGET_VERSION_FULL).nupkg)
RUNTIME_PACKS = $(RUNTIME_PACKS_IOS) $(RUNTIME_PACKS_TVOS) $(RUNTIME_PACKS_WATCHOS) $(RUNTIME_PACKS_MACOS)

REF_PACK_IOS = $(DOTNET_NUPKG_DIR)/$(IOS_NUGET).Ref.$(IOS_NUGET_VERSION_FULL).nupkg
REF_PACK_TVOS = $(DOTNET_NUPKG_DIR)/$(TVOS_NUGET).Ref.$(TVOS_NUGET_VERSION_FULL).nupkg
REF_PACK_WATCHOS = $(DOTNET_NUPKG_DIR)/$(WATCHOS_NUGET).Ref.$(WATCHOS_NUGET_VERSION_FULL).nupkg
REF_PACK_MACOS = $(DOTNET_NUPKG_DIR)/$(MACOS_NUGET).Ref.$(MACOS_NUGET_VERSION_FULL).nupkg
REF_PACKS = $(REF_PACK_IOS) $(REF_PACK_TVOS) $(REF_PACK_WATCHOS) $(REF_PACK_MACOS)

SDK_PACK_IOS = $(DOTNET_NUPKG_DIR)/$(IOS_NUGET).Sdk.$(IOS_NUGET_VERSION_FULL).nupkg
SDK_PACK_TVOS = $(DOTNET_NUPKG_DIR)/$(TVOS_NUGET).Sdk.$(TVOS_NUGET_VERSION_FULL).nupkg
SDK_PACK_WATCHOS = $(DOTNET_NUPKG_DIR)/$(WATCHOS_NUGET).Sdk.$(WATCHOS_NUGET_VERSION_FULL).nupkg
SDK_PACK_MACOS = $(DOTNET_NUPKG_DIR)/$(MACOS_NUGET).Sdk.$(MACOS_NUGET_VERSION_FULL).nupkg
SDK_PACKS = $(SDK_PACK_IOS) $(SDK_PACK_TVOS) $(SDK_PACK_WATCHOS) $(SDK_PACK_MACOS)

pack-ios: $(RUNTIME_PACKS_IOS) $(REF_PACK_IOS) $(SDK_PACK_IOS)
pack-tvos: $(RUNTIME_PACKS_TVOS) $(REF_PACK_TVOS) $(SDK_PACK_TVOS)
pack-watchos: $(RUNTIME_PACKS_WATCHOS) $(REF_PACK_WATCHOS) $(SDK_PACK_WATCHOS)
pack-macos: $(RUNTIME_PACKS_MACOS) $(REF_PACK_MACOS) $(SDK_PACK_MACOS)

TARGETS += $(RUNTIME_PACKS) $(REF_PACKS) $(SDK_PACKS)

ifdef ENABLE_DOTNET
all-local:: $(TARGETS) targets/Xamarin.Shared.Sdk.Versions.props
endif

# This tells NuGet to use the exact same dotnet version we've configured in Make.config
global.json: $(TOP)/global5.json
	$(CP) $< $@

clean-local::
	$(Q) rm -Rf $(DOTNET_NUPKG_DIR) $(DOTNET_FEED_DIR)
	$(Q) git clean -xfdq

.SECONDARY:
