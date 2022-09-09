using UnityEditor;
using UnityEditor.PackageManager.UI;
using UnityEngine;
using UnityEngine.UIElements;
using PackageInfo = UnityEditor.PackageManager.PackageInfo;

namespace Kogane.Internal
{
    [InitializeOnLoad]
    internal sealed class PackageManagerCopyNameButtonExtension
        : VisualElement,
          IPackageManagerExtension
    {
        private bool        m_isInitialized;
        private PackageInfo m_selectedPackageInfo;

        static PackageManagerCopyNameButtonExtension()
        {
            var extension = new PackageManagerCopyNameButtonExtension();
            PackageManagerExtensions.RegisterExtension( extension );
        }

        VisualElement IPackageManagerExtension.CreateExtensionUI()
        {
            m_isInitialized = false;
            return this;
        }

        void IPackageManagerExtension.OnPackageSelectionChange( PackageInfo packageInfo )
        {
            Initialize();

            m_selectedPackageInfo = packageInfo;
        }

        private void Initialize()
        {
            if ( m_isInitialized ) return;

            VisualElement root = this;

            while ( root is { parent: { } } )
            {
                root = root.parent;
            }

            var copyNameButton = new Button
            (
                () =>
                {
                    var packageName = m_selectedPackageInfo.name;

                    EditorGUIUtility.systemCopyBuffer = packageName;

                    Debug.Log( $"Copied! `{packageName}`" );
                }
            )
            {
                text = "Copy Name",
            };

            var removeButton = root.FindElement( x => x.name == "PackageRemoveCustomButton" );
            removeButton.parent.Insert( 0, copyNameButton );

            m_isInitialized = true;
        }

        void IPackageManagerExtension.OnPackageAddedOrUpdated( PackageInfo packageInfo )
        {
        }

        void IPackageManagerExtension.OnPackageRemoved( PackageInfo packageInfo )
        {
        }
    }
}